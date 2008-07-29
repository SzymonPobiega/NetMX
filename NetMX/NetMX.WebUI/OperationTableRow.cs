using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetMX;
using System.ComponentModel;
using System.Collections.Generic;
using NetMX.OpenMBean;

namespace NetMX.WebUI.WebControls
{
   internal sealed class OperationTableRow : TableRow, IMBeanFeatureControl, INamingContainer
   {
      #region Members
      private object[] _argumentValues;
      private readonly IMBeanServerConnection _connection;
      private readonly ObjectName _name;
      private readonly MBeanOperationInfo _operInfo;
      private readonly IOpenMBeanOperationInfo _openOperInfo;

		private bool _invokeMode;
		/// <summary>
		/// Tests if row is in invoke mode.
		/// </summary>
		public bool InvokeMode
		{
			get { return _invokeMode; }
      }
      private MBeanUIContext _context;
      private MBeanUIContext UIContext
      {
         get
         {
            if (_context == null)
            {
               _context = MBeanUIContext.GetInstance(this);
            }
            return _context;
         }
      }
      #endregion

      #region Controls
      private Button _invokeButton;
		private Button _cancelButton;
		private Table _arguments;
      private List<IValueEditControl> _argumentInputs;      
      #endregion

      internal OperationTableRow(ObjectName name, MBeanOperationInfo operInfo, IMBeanServerConnection connection)
      {
         ID = operInfo.Name;

         _name = name;
         _operInfo = operInfo;
         _connection = connection;
         _openOperInfo = operInfo as IOpenMBeanOperationInfo;
      }

      #region Subcontrol creation
      private void AddActionCell()
      {
         TableCell actionsCell = new TableCell();
         actionsCell.CssClass = CssClass;
         actionsCell.HorizontalAlign = HorizontalAlign.Center;

         _invokeButton = new Button();
         _invokeButton.Text = Resources.OperationTableRow.InvokeButton;
			_invokeButton.CssClass = UIContext.ButtonCssClass;
         _invokeButton.Click += OnInvoke;
         _invokeButton.EnableViewState = false;
         actionsCell.Controls.Add(_invokeButton);

			_cancelButton = new Button();
			_cancelButton.Text = Resources.OperationTableRow.CancelButton;
			_cancelButton.CssClass = UIContext.ButtonCssClass;
			_cancelButton.Click += OnCancel;
			_cancelButton.EnableViewState = false;
			actionsCell.Controls.Add(_cancelButton);

         Cells.Add(actionsCell);
      }
      private void AddArgumentsCell()
      {
         TableCell cell = new TableCell();
         cell.CssClass = CssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;
         _argumentInputs = new List<IValueEditControl>();
			_arguments = new Table();
			cell.Controls.Add(_arguments);
			for (int i = 0; i < _operInfo.Signature.Count; i++)         
         {
				MBeanParameterInfo paramInfo = _operInfo.Signature[i];
            IOpenMBeanParameterInfo openParamInfo = paramInfo as IOpenMBeanParameterInfo;
            
            TableRow row = new TableRow();
				TableCell nameCell = new TableCell();
				nameCell.ControlStyle.Width = Unit.Percentage(30);
				nameCell.Text = string.Format("{0}", paramInfo.Name);
				row.Cells.Add(nameCell);

            TableCell inputCell = new TableCell();            
            if (openParamInfo != null && openParamInfo.OpenType.Kind != OpenTypeKind.SimpleType)
            {
               Button editArgumentButton = new Button();
               editArgumentButton.CssClass = UIContext.ButtonCssClass;
               editArgumentButton.ID = _operInfo.Name + "__" + paramInfo.Name;
               editArgumentButton.EnableViewState = false;
               editArgumentButton.Click += OnArgumentEditButton;
               _argumentInputs.Add(null);
               inputCell.Controls.Add(editArgumentButton);
            }
            else
            {
               IValueEditControl argumentBox = ValueEditControlFactory.CreateValueEditControl(openParamInfo);
               argumentBox.CssClass = CssClass;
               argumentBox.ID = _operInfo.Name + "__" + paramInfo.Name;
               argumentBox.EnableViewState = false;
               _argumentInputs.Add(argumentBox);
               inputCell.Controls.Add((Control)argumentBox);
            }				            				
				inputCell.ControlStyle.Width = Unit.Percentage(40);
				row.Cells.Add(inputCell);

				TableCell descrCell = new TableCell();
				descrCell.ControlStyle.Width = Unit.Percentage(30);
				descrCell.Text = string.Format("({0})", paramInfo.Description);
				row.Cells.Add(descrCell);
				_arguments.Rows.Add(row);				
         }			
         Cells.Add(cell);
      }
      private void AddCell(string value, bool center)
      {
         TableCell cell = new TableCell();
         cell.CssClass = CssClass;
         cell.Text = value;
         cell.HorizontalAlign = center ? HorizontalAlign.Center : HorizontalAlign.Left;
         Cells.Add(cell);
      }
      #endregion

      #region IMBeanFeatureControl Members
      public void SetUIState(bool enabled)
      {
         _invokeButton.Enabled = enabled;
      }
      public object Selector
      {
         get { return new MBeanOperationSelector(_operInfo.Name, null); }
      }
      public void SetOpenTypeValue(object currentSelector, object value)
      {
         MBeanOperationSelector typedSelector = (MBeanOperationSelector) currentSelector;
         SetStoredArgumentValue(typedSelector.ArgumentName, value);
         OnChangeUIState(true);
      }
      #endregion

      #region Events
      /// <summary>
      /// Raised when action performed by user should change the behavior of other controls. In response for 
      /// this event, the main control (<see cref="MBeanUI"/>) executes 
      /// </summary>
      public event EventHandler<ChangeUIStateEventArgs> ChangeUIState;
      private void OnChangeUIState(bool enabled)
      {
         if (ChangeUIState != null)
         {
            EventHandler<ChangeUIStateEventArgs> handler = ChangeUIState;
            handler(this, new ChangeUIStateEventArgs(enabled, new MBeanOperationSelector(_operInfo.Name, null)));
         }
      }
      /// <summary>
      /// Raised when user wants to view or edit <see cref="TabularType"/> or <see cref="CompositeType"/> value.
      /// </summary>
      public event EventHandler<ViewEditOpenTypeEventArgs> ViewEditOpenType;
      private void OnViewEditOpenType(ViewEditOpenTypeEventArgs args)
      {
         if (ViewEditOpenType != null)
         {
            EventHandler<ViewEditOpenTypeEventArgs> handler = ViewEditOpenType;
            handler(this, args);
         }
      }
      #endregion

      #region Overridden  
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page.RegisterRequiresControlState(this);
		}
		protected override void LoadControlState(object savedState)
		{
			object[] state = (object[])savedState;
			_invokeMode = (bool)state[1];
		   _argumentValues = (object[]) state[2];
			base.LoadControlState(state[0]);
		}
		protected override object SaveControlState()
		{
			return new[] { base.SaveControlState(), _invokeMode, _argumentValues };
		}
      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         CssClass = UIContext.OperationTableCssClass;
         AddCell(_operInfo.Name, false);
         AddCell(_operInfo.Description, false);
         AddCell(_operInfo.Impact.ToString(), true);
         AddArgumentsCell();
         AddActionCell();
      }
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);         
			_arguments.Visible = _invokeMode;
			_cancelButton.Visible = _invokeMode;
		}
      #endregion

      #region Event handlers
      private void OnArgumentEditButton(object sender, EventArgs e)
      {
         string[] tmp = ((Control) sender).ID.Split(new[] {"__"}, StringSplitOptions.RemoveEmptyEntries);
         OpenType valueOpenType;
         object value = GetStoredArgumentValue(tmp[1], out valueOpenType);
         OnViewEditOpenType(new ViewEditOpenTypeEventArgs(true, value, valueOpenType,
                                                                   new MBeanOperationSelector(_operInfo.Name, tmp[1]),
                                                                   _operInfo.Description));
      }
      private void OnInvoke(object sender, EventArgs e)
      {
			if (_invokeMode)
			{
				object[] arguments = new object[_argumentInputs.Count];
				for (int i = 0; i < arguments.Length; i++)
				{
               if (_argumentInputs[i] == null)
               {
                  arguments[i] = _argumentValues[i];
               }
               else
               {
                  TypeConverter converter = TypeDescriptor.GetConverter(Type.GetType(_operInfo.Signature[i].Type, true));
                  arguments[i] = converter.ConvertFromString(_argumentInputs[i].Value);
               }
				}
				object o = _connection.Invoke(_name, _operInfo.Name, arguments);
			   _argumentValues = null;
            if (_openOperInfo != null && o != null)
            {
               if (_openOperInfo.ReturnOpenType != SimpleType.Void)
               {
                  OnViewEditOpenType(new ViewEditOpenTypeEventArgs(false, o, _openOperInfo.ReturnOpenType,
                                                                   new MBeanOperationSelector(_operInfo.Name, null),
                                                                   _operInfo.Description));                  
               }               
            }            
            else if (_openOperInfo == null || _openOperInfo.ReturnOpenType != SimpleType.Void)
            {
               string text = o != null ? o.ToString() : "null";
               Page.ClientScript.RegisterStartupScript(typeof(OperationTableRow),"SHOW_RESULTS",string.Format(
@"
<script language=""javascript"">
   alert('{0}: {1}');
</script>
", Resources.OperationTableRow.ResultsText, text));
            }
			   _invokeMode = false;
            OnChangeUIState(true);
			}
			else
			{
				OnChangeUIState(false);
				_invokeMode = true;
			}
      }
		private void OnCancel(object sender, EventArgs e)
		{
			_invokeMode = false;
		   _argumentValues = null;
		}
      #endregion

      #region Utility
      private object GetStoredArgumentValue(string name, out OpenType openType)
      {         
         for (int i = 0; i < _operInfo.Signature.Count; i++)
         {
            if (_operInfo.Signature[i].Name == name)
            {
               openType = ((IOpenMBeanParameterInfo) _openOperInfo.Signature[i]).OpenType;
               return _argumentValues != null ? _argumentValues[i] : null;
            }
         }
         throw new ArgumentException("No such parameter name.");
      }
      private void SetStoredArgumentValue(string name, object value)
      {
         if (_argumentValues == null)
         {
            _argumentValues = new object[_operInfo.Signature.Count];
         }
         for (int i = 0; i < _operInfo.Signature.Count; i++)
         {
            if (_operInfo.Signature[i].Name == name)
            {
               _argumentValues[i] = value;
            }
         }
      }
      #endregion

      #region Selector class
      [Serializable]
      private sealed class MBeanOperationSelector
      {
         private readonly string _name;
         private readonly string _argumentName;

         public MBeanOperationSelector(string name, string argumentName)
         {
            _name = name;
            _argumentName = argumentName;
         }

         public string Name
         {
            get { return _name; }
         }

         public string ArgumentName
         {
            get { return _argumentName;  }
         }

         public override int GetHashCode()
         {
            return Name.GetHashCode();
         }
         public override bool Equals(object obj)
         {
            MBeanOperationSelector other = obj as MBeanOperationSelector;
            return other != null && Name.Equals(other.Name);
         }
      }
      #endregion      
   }
}