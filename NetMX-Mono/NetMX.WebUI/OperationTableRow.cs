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

namespace NetMX.WebUI.WebControls
{
   public class OperationTableRow : TableRow
   {
      private IMBeanServerConnection _connection;
      private ObjectName _name;
      private MBeanOperationInfo _operInfo;

		private bool _invokeMode;
		/// <summary>
		/// Tests if row is in invoke mode.
		/// </summary>
		public bool InvokeMode
		{
			get { return _invokeMode; }
		}
      
      #region Controls
      private Button _invokeButton;
		private Button _cancelButton;
		private Table _arguments;
      private List<TextBox> _argumentInputs;      
      #endregion

		internal OperationTableRow(ObjectName name, MBeanOperationInfo operInfo, IMBeanServerConnection connection, string rowCssClass, string buttonCssClass)
      {
         _name = name;
         _operInfo = operInfo;
         _connection = connection;
			this.CssClass = rowCssClass;

         AddCell(operInfo.Name, false);
         AddCell(operInfo.Description, false);
         AddCell(operInfo.Impact.ToString(), true);
         AddArgumentsCell();
			AddActionCell(buttonCssClass);
      }
      private void AddActionCell(string buttonCssClass)
      {
         TableCell actionsCell = new TableCell();
         actionsCell.CssClass = this.CssClass;
         actionsCell.HorizontalAlign = HorizontalAlign.Center;

         _invokeButton = new Button();
         _invokeButton.Text = Resources.OperationTableRow.InvokeButton;
			_invokeButton.CssClass = buttonCssClass;
         _invokeButton.Click += new EventHandler(OnInvoke);
         _invokeButton.EnableViewState = false;
         actionsCell.Controls.Add(_invokeButton);

			_cancelButton = new Button();
			_cancelButton.Text = Resources.OperationTableRow.CancelButton;
			_cancelButton.CssClass = buttonCssClass;
			_cancelButton.Click += new EventHandler(OnCancel);
			_cancelButton.EnableViewState = false;
			actionsCell.Controls.Add(_cancelButton);

         this.Cells.Add(actionsCell);
      }
      private void AddArgumentsCell()
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;
         _argumentInputs = new List<TextBox>();
			_arguments = new Table();
			cell.Controls.Add(_arguments);
			for (int i = 0; i < _operInfo.Signature.Count; i++)         
         {
				MBeanParameterInfo paramInfo = _operInfo.Signature[i];
				TableRow row = new TableRow();
				TableCell nameCell = new TableCell();
				nameCell.ControlStyle.Width = Unit.Percentage(30);
				nameCell.Text = string.Format("{0}", paramInfo.Name);
				row.Cells.Add(nameCell);

				TableCell inputCell = new TableCell();
				TextBox argumentBox = new TextBox();
				argumentBox.CssClass = this.CssClass;
				argumentBox.ID = _operInfo.Name + "__" + paramInfo.Name;
				argumentBox.EnableViewState = false;
				_argumentInputs.Add(argumentBox);
				inputCell.Controls.Add(argumentBox);
				inputCell.ControlStyle.Width = Unit.Percentage(40);
				row.Cells.Add(inputCell);

				TableCell descrCell = new TableCell();
				descrCell.ControlStyle.Width = Unit.Percentage(30);
				descrCell.Text = string.Format("({0})", paramInfo.Description);
				row.Cells.Add(descrCell);
				_arguments.Rows.Add(row);

				//cell.Controls.Add(new LiteralControl(string.Format("{0}:", paramInfo.Name)));
				//TextBox argumentBox = new TextBox();
				//argumentBox.CssClass = this.CssClass;
				//argumentBox.ID = _operInfo.Name +"__" + paramInfo.Name;
				//argumentBox.EnableViewState = false;
				//_argumentInputs.Add(argumentBox);
				//cell.Controls.Add(argumentBox);
				//cell.Controls.Add(new LiteralControl(string.Format("({0})", paramInfo.Description)));
				//if (i < _operInfo.Signature.Count - 1)
				//{
				//   cell.Controls.Add(new LiteralControl("<br />"));
				//}
         }			
         this.Cells.Add(cell);
      }
      private void AddCell(string value, bool center)
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.Text = value;
         cell.HorizontalAlign = center ? HorizontalAlign.Center : HorizontalAlign.Left;
         this.Cells.Add(cell);
      }

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
			base.LoadControlState(state[0]);
		}
		protected override object SaveControlState()
		{
			return new object[] { base.SaveControlState(), _invokeMode };
		}
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			_arguments.Visible = _invokeMode;
			_cancelButton.Visible = _invokeMode;
		}
      #endregion

      #region Event handlers
      private void OnInvoke(object sender, EventArgs e)
      {
			if (_invokeMode)
			{
				object[] arguments = new object[_argumentInputs.Count];
				for (int i = 0; i < arguments.Length; i++)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(Type.GetType(_operInfo.Signature[i].Type, true));
					arguments[i] = converter.ConvertFromString(_argumentInputs[i].Text);
				}
				_connection.Invoke(_name, _operInfo.Name, arguments);
				_invokeMode = false;
			}
			else
			{				
				_invokeMode = true;
			}
      }
		private void OnCancel(object sender, EventArgs e)
		{
			_invokeMode = false;
		}
      #endregion
   }
}