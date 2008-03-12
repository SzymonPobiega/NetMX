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

namespace Controls
{
   public class OperationTableRow : TableRow
   {
      private IMBeanServerConnection _connection;
      private ObjectName _name;
      private MBeanOperationInfo _operInfo;
      
      #region Controls
      private Button _invokeButton;
      private List<TextBox> _argumentInputs;      
      #endregion

      public OperationTableRow(ObjectName name, MBeanOperationInfo operInfo, IMBeanServerConnection connection)
      {
         _name = name;
         _operInfo = operInfo;
         _connection = connection;
         this.CssClass = "Operation";

         AddCell(operInfo.Name, false);
         AddCell(operInfo.Description, false);
         AddCell(operInfo.Impact.ToString(), true);
         AddArgumentsCell();
         AddActionCell();
      }
      private void AddActionCell()
      {
         TableCell actionsCell = new TableCell();
         actionsCell.CssClass = this.CssClass;
         actionsCell.HorizontalAlign = HorizontalAlign.Center;

         _invokeButton = new Button();
         _invokeButton.Text = "Invoke";
         _invokeButton.CssClass = this.CssClass;
         _invokeButton.Click += new EventHandler(OnInvoke);
         _invokeButton.EnableViewState = false;
         actionsCell.Controls.Add(_invokeButton);

         this.Cells.Add(actionsCell);
      }
      private void AddArgumentsCell()
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;
         _argumentInputs = new List<TextBox>();
         foreach (MBeanParameterInfo paramInfo in _operInfo.Signature)
         {
            TextBox argumentBox = new TextBox();
            argumentBox.CssClass = this.CssClass;
            argumentBox.ID = _operInfo.Name +"__" + paramInfo.Name;
            argumentBox.EnableViewState = false;
            _argumentInputs.Add(argumentBox);
            cell.Controls.Add(argumentBox);
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
      #endregion

      #region Event handlers
      private void OnInvoke(object sender, EventArgs e)
      {
         object[] arguments =new object[_argumentInputs.Count];
         for (int i = 0; i < arguments.Length; i++)
         {
            TypeConverter converter = TypeDescriptor.GetConverter(Type.GetType(_operInfo.Signature[i].Type, true));
            arguments[i] = converter.ConvertFromString(_argumentInputs[i].Text);
         }
         _connection.Invoke(_name, _operInfo.Name, arguments);
      }
      #endregion
   }
}