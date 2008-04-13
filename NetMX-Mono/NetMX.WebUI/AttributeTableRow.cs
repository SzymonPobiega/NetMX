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

namespace NetMX.WebUI.WebControls
{   
   public class AttributeTableRow : TableRow
   {
      private IMBeanServerConnection _connection;
      private ObjectName _name;
      private MBeanAttributeInfo _attrInfo;

      private bool _editMode;
      /// <summary>
      /// Tests if row is in edit mode.
      /// </summary>
      public bool EditMode
      {
         get { return _editMode; }
      }

      #region Controls
      private Button _editButton;
      private Button _updateButton;
      private Button _cancelButton;
      private TextBox _input;
      private LiteralControl _literal;
      #endregion

		internal AttributeTableRow(ObjectName name, MBeanAttributeInfo attrInfo, IMBeanServerConnection connection, string rowCssClass, string buttonCssClass)
      {
         _name = name;
         _attrInfo = attrInfo;
         _connection = connection;
         this.CssClass = rowCssClass;

         AddCell(attrInfo.Name, false);
         AddCell(attrInfo.Description, false);
			AddCell(string.Format("{0}{1}", attrInfo.Readable ? Resources.AttributeTableRow.ReadableSymbol : "", attrInfo.Writable ? Resources.AttributeTableRow.WritableSymbol : ""), true);
         AddValueCell();
         AddActionsCell(attrInfo.Name, buttonCssClass);
      }
		private void AddActionsCell(string name, string buttonCssClass)
      {
         TableCell actionsCell = new TableCell();
         actionsCell.CssClass = this.CssClass;
         actionsCell.HorizontalAlign = HorizontalAlign.Center;

         if (_attrInfo.Writable && _attrInfo.Readable)
         {
            _editButton = new Button();
				_editButton.Text = Resources.AttributeTableRow.EditButton;
				_editButton.CssClass = buttonCssClass;
            _editButton.Click += new EventHandler(OnEdit);
            _editButton.EnableViewState = false;
            actionsCell.Controls.Add(_editButton);

            _updateButton = new Button();
				_updateButton.Text = Resources.AttributeTableRow.UpdateButton;
				_updateButton.CssClass = buttonCssClass;
            _updateButton.Click += new EventHandler(OnUpdate);
            _updateButton.EnableViewState = false;
            actionsCell.Controls.Add(_updateButton);

            _cancelButton = new Button();
				_cancelButton.Text = Resources.AttributeTableRow.CancelButton;
				_cancelButton.CssClass = buttonCssClass;
            _cancelButton.Click += new EventHandler(OnCancel);
            _cancelButton.EnableViewState = false;
            actionsCell.Controls.Add(_cancelButton);
         }
         this.Cells.Add(actionsCell);
      }
      private void AddValueCell()
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;

         _input = new TextBox();
         _input.CssClass = this.CssClass;
         _input.EnableViewState = false;
         cell.Controls.Add(_input);

         _literal = new LiteralControl();
         cell.Controls.Add(_literal);

         this.Cells.Add(cell);
      }
      private void AddCell(string value, bool center)
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.Text = value;
         if (center)
         {
            cell.HorizontalAlign = HorizontalAlign.Center;
         }
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
         _editMode = (bool)state[1];
         base.LoadControlState(state[0]);
      }
      protected override object SaveControlState()
      {
         return new object[] { base.SaveControlState(), _editMode };
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
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
            _input.Visible = false;
            if (_updateButton != null && _cancelButton != null)
            {
               _updateButton.Visible = false;
               _cancelButton.Visible = false;
            }
         }
         if (_attrInfo.Readable)
         {
            object value = _connection.GetAttribute(_name, _attrInfo.Name);
            _input.Text = value != null ? value.ToString() : "";
            _literal.Text = HttpUtility.HtmlEncode(_input.Text);
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
      }
      private void OnUpdate(object sender, EventArgs e)
      {
         _editMode = false;
         TypeConverter converter = TypeDescriptor.GetConverter(Type.GetType(_attrInfo.Type, true));
         _connection.SetAttribute(_name, _attrInfo.Name, converter.ConvertFromString(_input.Text));
      }
      private void OnEdit(object sender, EventArgs e)
      {
         _editMode = true;
      }
      #endregion
   }
}