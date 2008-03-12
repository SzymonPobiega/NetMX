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

namespace Controls
{
   public class MBeanUI2 : CompositeControl
   {
      private string _mBeanServerProxyID;
      /// <summary>
      /// ID of MBeanServerProxy control
      /// </summary>
      public string MBeanServerProxyID
      {
         get { return _mBeanServerProxyID; }
         set { _mBeanServerProxyID = value; }
      }
      //private ObjectName _objectName;
      /// <summary>
      /// ObjectName of displayed/edited MBean
      /// </summary>
      public string ObjectName
      {
         get 
			{
				return (string) ViewState["ObjectName"];
			}
			set
			{
				ViewState["ObjectName"] = value;
			}
      }
      private MBeanServerProxy _proxy;
      private MBeanServerProxy Proxy
      {
         get
         {
            if (_proxy == null)
            {
               Control container = this.NamingContainer;
               while (container != null)
               {
                  _proxy = (MBeanServerProxy)container.FindControl(MBeanServerProxyID);
                  if (_proxy != null)
                  {
                     break;
                  }
                  else
                  {
                     container = container.NamingContainer;
                  }
               }
            }
            return _proxy;
         }
      }
      protected override void CreateChildControls()
      {
         base.CreateChildControls();

         MBeanInfo info = Proxy.ServerConnection.GetMBeanInfo(new ObjectName(ObjectName));

         Label generalInfoTitle = new Label();
         generalInfoTitle.Text = "General information";
         generalInfoTitle.CssClass = "SectionTitle";
         this.Controls.Add(generalInfoTitle);

			Button refreshButton = new Button();
			refreshButton.Text = "Refresh";
			this.Controls.Add(refreshButton);

         Table generalInfo = new Table();
         generalInfo.CssClass = "GeneralInfo";
         generalInfo.ControlStyle.Width = Unit.Percentage(100);
         AddGeneralInfoItem(generalInfo, "ObjectName", ObjectName);
         AddGeneralInfoItem(generalInfo, "Description", info.Description);
         AddGeneralInfoItem(generalInfo, "Class", info.ClassName);
         this.Controls.Add(generalInfo);         

         Label attributesTitle = new Label();
         attributesTitle.Text = "Attributes";
         attributesTitle.CssClass = "SectionTitle";
         this.Controls.Add(attributesTitle);

         Table attributes = new Table();
         attributes.CssClass = "Attribute";
         attributes.ControlStyle.Width = Unit.Percentage(100);
         attributes.Rows.Add(CreateAttributesHeader());         
         foreach (MBeanAttributeInfo attrInfo in info.Attributes)
         {
				AttributeTableRow attributeRow = new AttributeTableRow(new ObjectName(ObjectName), attrInfo, Proxy.ServerConnection);
            attributes.Rows.Add(attributeRow);
         }
         this.Controls.Add(attributes);

         Label operationTitle = new Label();
         operationTitle.Text = "Operations";
         operationTitle.CssClass = "SectionTitle";
         this.Controls.Add(operationTitle);

         Table operations = new Table();
         operations.CssClass = "Operation";
         operations.ControlStyle.Width = Unit.Percentage(100);
         operations.Rows.Add(CreateOperationsHeader());
         foreach (MBeanOperationInfo operInfo in info.Operations)
         {
				OperationTableRow operationRow = new OperationTableRow(new ObjectName(ObjectName), operInfo, Proxy.ServerConnection);
            operations.Rows.Add(operationRow);
         }
         this.Controls.Add(operations);
      }
      private void AddGeneralInfoItem(Table table, string name, string value)
      {
         TableRow row = new TableRow();
         row.CssClass = "GeneralInfo";

         TableCell nameCell = new TableCell();
         nameCell.CssClass = "GeneralInfoName";
         nameCell.Text = name;
         nameCell.ControlStyle.Width = Unit.Percentage(20);
         row.Cells.Add(nameCell);

         TableCell valueCell = new TableCell();
         valueCell.CssClass = "GeneralInfoValue";
         valueCell.Text = value;
         valueCell.ControlStyle.Width = Unit.Percentage(80);
         row.Cells.Add(valueCell);

         table.Rows.Add(row);
      }
      private TableHeaderRow CreateAttributesHeader()
      {
         TableHeaderRow attrHeader = new TableHeaderRow();
         attrHeader.CssClass = "Attribute";
         AddAttributesHeaderCell(attrHeader, "Name", 20);
         AddAttributesHeaderCell(attrHeader, "Description", 20);
         AddAttributesHeaderCell(attrHeader, "Access", 10);
         AddAttributesHeaderCell(attrHeader, "Value", 25);
         AddAttributesHeaderCell(attrHeader, "Actions", 25);
         return attrHeader;
      }
      private TableHeaderRow CreateOperationsHeader()
      {
         TableHeaderRow operHeader = new TableHeaderRow();
         operHeader.CssClass = "Operation";
         AddOperationsHeaderCell(operHeader, "Name", 20);
         AddOperationsHeaderCell(operHeader, "Description", 20);
         AddOperationsHeaderCell(operHeader, "Impact", 10);
         AddOperationsHeaderCell(operHeader, "Arguments", 35);
         AddOperationsHeaderCell(operHeader, "Actions", 15);
         return operHeader;
      }
      private void AddOperationsHeaderCell(TableHeaderRow row, string name, double percentSize)
      {
         TableHeaderCell cell = new TableHeaderCell();
         cell.CssClass = "Operation";
         cell.ControlStyle.Width = Unit.Percentage(percentSize);
         cell.Text = name;
         row.Cells.Add(cell);
      }
      private void AddAttributesHeaderCell(TableHeaderRow row, string name, double percentSize)
      {
         TableHeaderCell cell = new TableHeaderCell();
         cell.CssClass = "Attribute";
         cell.ControlStyle.Width = Unit.Percentage(percentSize);
         cell.Text = name;
         row.Cells.Add(cell);
      }
   }
}