using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetMX;
using System.ComponentModel;
using NetMX.OpenMBean;

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// A control which presents an MBean attribute in form of a row of a table.
   /// </summary>
   internal sealed class AttributeTableRow : TableRow, INamingContainer, IMBeanFeatureControl
   {
      #region Members
      private object _openTypeValue;      
      private readonly IMBeanServerConnection _connection;
      private readonly ObjectName _name;
      private readonly MBeanAttributeInfo _attrInfo;
      private readonly IOpenMBeanAttributeInfo _openAttrInfo;

      private bool _editMode;
      /// <summary>
      /// Tests if row is in edit mode.
      /// </summary>
      internal bool EditMode
      {
         get { return _editMode; }
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
      private Button _editButton;
      private Button _updateButton;
      private Button _cancelButton;
      private IValueEditControl _input;
      private Button _editOpenType;
      private LiteralControl _literal;
      #endregion

      #region Constructor
      internal AttributeTableRow(ObjectName name, MBeanAttributeInfo attrInfo, IMBeanServerConnection connection)
      {
         ID = attrInfo.Name;

         _name = name;         
         _attrInfo = attrInfo;
         _openAttrInfo = attrInfo as IOpenMBeanAttributeInfo;
         _connection = connection;         
      }
      #endregion

      #region Subcontrol creation
      private void AddActionsCell()
      {
         TableCell actionsCell = new TableCell {CssClass = UIContext.AttributeTableCssClass, HorizontalAlign = HorizontalAlign.Center};

         if (_attrInfo.Writable && _attrInfo.Readable)
         {
            _editButton = new Button();

            _editButton.Text = Resources.AttributeTableRow.EditButton;
            _editButton.CssClass = UIContext.ButtonCssClass;
            _editButton.EnableViewState = false;                             
            _editButton.Click += OnEdit;            
            actionsCell.Controls.Add(_editButton);

            _updateButton = new Button();

            _updateButton.Text = Resources.AttributeTableRow.UpdateButton;
            _updateButton.CssClass = UIContext.ButtonCssClass;
            _updateButton.EnableViewState = false;                               
            _updateButton.Click += OnUpdate;            
            actionsCell.Controls.Add(_updateButton);

            _cancelButton = new Button();

            _cancelButton.Text = Resources.AttributeTableRow.CancelButton;
            _cancelButton.CssClass = UIContext.ButtonCssClass;
            _cancelButton.EnableViewState = false;                               
            _cancelButton.Click += OnCancel;            
            actionsCell.Controls.Add(_cancelButton);
         }
         Cells.Add(actionsCell);
      }
      private void AddValueCell()
      {
         TableCell cell = new TableCell();
         cell.CssClass = UIContext.AttributeTableCssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;

         if (_openAttrInfo != null && _openAttrInfo.OpenType.Kind != OpenTypeKind.SimpleType)
         {
            _editOpenType = new Button();
            if (_openAttrInfo.Writable)
            {
               _editOpenType.Click += OnEditOpenType;
               _editOpenType.Text = Resources.AttributeTableRow.SetEditButton;
            }
            else
            {
               _editOpenType.Click += OnViewOpenType;
               _editOpenType.Text = Resources.AttributeTableRow.ViewButton;
            }            
            _editOpenType.CssClass = UIContext.ButtonCssClass;
            _editOpenType.EnableViewState = false;            
            cell.Controls.Add(_editOpenType);
         }
         else
         {
            _input = ValueEditControlFactory.CreateValueEditControl(_openAttrInfo);
            _input.CssClass = UIContext.AttributeTableCssClass;
            _input.EnableViewState = false;
            cell.Controls.Add((Control)_input);
         }

         _literal = new LiteralControl();
         cell.Controls.Add(_literal);

         Cells.Add(cell);
      }      
      private void AddCell(string value, bool center)
      {
         TableCell cell = new TableCell();
         cell.CssClass = UIContext.AttributeTableCssClass;
         cell.Text = value;
         if (center)
         {
            cell.HorizontalAlign = HorizontalAlign.Center;
         }
         Cells.Add(cell);
      }
      #endregion
      
      #region IMBeanFeatureControl Members
      public void SetUIState(bool enabled)
      {
         if (_editButton != null)
         {
            _editButton.Enabled = enabled;
         }
      }      
      public object Selector
      {
         get { return new MBeanAtributeSelector(_attrInfo.Name); }
      }      
      public void SetOpenTypeValue(object currentSelector, object value)
      {
         _openTypeValue = value;
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
            handler(this, new ChangeUIStateEventArgs(enabled, new MBeanAtributeSelector(_attrInfo.Name)));
         }
      }
      /// <summary>
      /// Raised when user wants to view or edit <see cref="TabularType"/> or <see cref="CompositeType"/> value.
      /// </summary>
      public event EventHandler<ViewEditOpenTypeEventArgs> ViewEditOpenType;
      private void OnViewEditOpenType(ViewEditOpenTypeEventArgs args)
      {
         OnChangeUIState(false);
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
         base.LoadControlState(state[0]);
         _editMode = (bool)state[1];
         _openTypeValue = state[2];
      }
      protected override object SaveControlState()
      {
         return new object[] { base.SaveControlState(), _editMode, _openTypeValue };
      }
      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         AddCell(_attrInfo.Name, false);
         AddCell(_attrInfo.Description, false);
         AddCell(string.Format("{0}{1}", _attrInfo.Readable ? Resources.AttributeTableRow.ReadableSymbol : "", _attrInfo.Writable ? Resources.AttributeTableRow.WritableSymbol : ""), true);
         AddValueCell();
         AddActionsCell();
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
         CssClass = UIContext.AttributeTableCssClass;
         if (_editMode)
         {
            _literal.Visible = false;
            if (_editButton != null)
            {
               _editButton.Visible = false;
            }
         }
         else
         {
            if (_input != null)
            {
               _input.Visible = false;
            }
            if (_editOpenType != null && _attrInfo.Writable)
            {
               _editOpenType.Visible = false;
            }
            if (_updateButton != null && _cancelButton != null)
            {
               _updateButton.Visible = false;
               _cancelButton.Visible = false;
            }
         }
         if (_attrInfo.Readable)
         {
            try
            {
               object value = _connection.GetAttribute(_name, _attrInfo.Name);
               if (_input != null)
               {
                  _input.Value = value != null ? value.ToString() : "";
                  _literal.Text = HttpUtility.HtmlEncode(_input.Value);
               }
            }
            catch (AttributeNotFoundException)
            {
               Visible = false;
            }
         }
         else
         {
				_literal.Text = Resources.AttributeTableRow.UnreadableValue;
         }
      }
      #endregion      

      #region Event handlers
      private void OnCancel(object sender, EventArgs e)
      {
         _editMode = false;
         _openTypeValue = null;
         OnChangeUIState(true);
      }
      private void OnUpdate(object sender, EventArgs e)
      {
         _editMode = false;
         if (_openAttrInfo != null && _openAttrInfo.OpenType.Kind != OpenTypeKind.SimpleType)
         {
            _connection.SetAttribute(_name, _attrInfo.Name, _openTypeValue);
         }
         else
         {
            TypeConverter converter = TypeDescriptor.GetConverter(Type.GetType(_attrInfo.Type, true));
            _connection.SetAttribute(_name, _attrInfo.Name, converter.ConvertFromString(_input.Value));  
         }
         OnChangeUIState(true);
      }
      private void OnEdit(object sender, EventArgs e)
      {
         _editMode = true;
         OnChangeUIState(false);
      }
      private void OnEditOpenType(object sender, EventArgs e)
      {
         if (_openTypeValue == null)
         {
            _openTypeValue = _connection.GetAttribute(_name, _attrInfo.Name);
         }
         ViewOrEditOpenType(true, _openTypeValue);
      }
      private void OnViewOpenType(object sender, EventArgs e)
      {
         ViewOrEditOpenType(false, _connection.GetAttribute(_name, _attrInfo.Name));
      }
      private void ViewOrEditOpenType(bool edit, object value)
      {         
         OnViewEditOpenType(new ViewEditOpenTypeEventArgs(edit, value,
             _openAttrInfo.OpenType, new MBeanAtributeSelector(_attrInfo.Name), _attrInfo.Description));         
      }
      #endregion

      #region Selector class
      [Serializable]
      private sealed class MBeanAtributeSelector
      {
         private readonly string _name;

         public MBeanAtributeSelector(string name)
         {
            _name = name;
         }

         public string Name
         {
            get { return _name; }
         }

         public override int GetHashCode()
         {
            return Name.GetHashCode();
         }
         public override bool Equals(object obj)
         {
            MBeanAtributeSelector other = obj as MBeanAtributeSelector;
            return other != null && Name.Equals(other.Name);            
         }
      }
      #endregion      
   }
}