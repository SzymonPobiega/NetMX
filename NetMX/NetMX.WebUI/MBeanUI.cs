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
	public class MBeanUI : CompositeControl
	{
		#region Data source properties
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
				return (string)ViewState["ObjectName"];
			}
			set
			{				
				ViewState["ObjectName"] = value;
				if (value != null)
				{
					CreateControls();
				}
			}
		}
		#endregion

		#region Appearance properties
		private string _buttonCssClass;
		/// <summary>
		/// Css class of buttons
		/// </summary>
		[
		Category("Appearance"),
		DefaultValue("")
		]
		public string ButtonCssClass
		{
			get { return _buttonCssClass; }
			set { _buttonCssClass = value; }
		}
		private int _tableCellSpacing;
		/// <summary>
		/// Cell-spacing of rendered tables (general info, attributes and operations)
		/// </summary>
		[
		Category("Appearance"),
		DefaultValue(0)
		]
		public int TableCellSpacing
		{
			get { return _tableCellSpacing; }
			set { _tableCellSpacing = value; }
		}
		private int _tableCellPadding;
		/// <summary>
		/// Cell-padding of rendered tables (general info, attributes and operations)
		/// </summary>
		[
		Category("Appearance"),
		DefaultValue(0)
		]
		public int TableCellPadding
		{
			get { return _tableCellPadding; }
			set { _tableCellPadding = value; }
		}
		private string _attributeTableCssClass;
		[
		Category("Appearance"),
		DefaultValue("")
		]
		public string AttributeTableCssClass
		{
			get { return _attributeTableCssClass; }
			set { _attributeTableCssClass = value; }
		}
		private string _operationTableCssClass;
		[
		Category("Appearance"),
		DefaultValue("")
		]
		public string OperationTableCssClass
		{
			get { return _operationTableCssClass; }
			set { _operationTableCssClass = value; }
		}
		#endregion

		#region Overridden
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Div;
			}
		}
		protected override void CreateChildControls()
		{
			base.CreateChildControls();						
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (ObjectName != null)
			{
				CreateControls();
			}
		}
		#endregion

		#region Utility
		private void CreateControls()
		{
			MBeanInfo info = Proxy.ServerConnection.GetMBeanInfo(new ObjectName(ObjectName));

			Label generalInfoTitle = new Label();
			generalInfoTitle.Text = Resources.MBeanUI.GeneralInformationSection + "&nbsp;&nbsp;";
			generalInfoTitle.CssClass = "SectionTitle";
			this.Controls.Add(generalInfoTitle);

			Button refreshButton = new Button();
			refreshButton.Text = Resources.MBeanUI.RefreshButton;
			refreshButton.CssClass = ButtonCssClass;
			this.Controls.Add(refreshButton);

			Table generalInfo = new Table();
			generalInfo.CellPadding = TableCellPadding;
			generalInfo.CellSpacing = TableCellSpacing;
			generalInfo.CssClass = "GeneralInfo";
			generalInfo.ControlStyle.Width = Unit.Percentage(100);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationObjectName, ObjectName);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationDescription, info.Description);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationClass, info.ClassName);
			this.Controls.Add(generalInfo);

			Label attributesTitle = new Label();
			attributesTitle.Text = Resources.MBeanUI.AttributesSection;
			attributesTitle.CssClass = "SectionTitle";
			this.Controls.Add(attributesTitle);

			Table attributes = new Table();
			attributes.CellPadding = TableCellPadding;
			attributes.CellSpacing = TableCellSpacing;
			attributes.CssClass = AttributeTableCssClass;
			attributes.ControlStyle.Width = Unit.Percentage(100);
			attributes.Rows.Add(CreateAttributesHeader());
			foreach (MBeanAttributeInfo attrInfo in info.Attributes)
			{
				AttributeTableRow attributeRow = new AttributeTableRow(new ObjectName(ObjectName), attrInfo, Proxy.ServerConnection, "Attribute", ButtonCssClass);
				attributes.Rows.Add(attributeRow);
			}
			this.Controls.Add(attributes);

			Label operationTitle = new Label();
			operationTitle.Text = Resources.MBeanUI.OperationsSection;
			operationTitle.CssClass = "SectionTitle";
			this.Controls.Add(operationTitle);

			Table operations = new Table();
			operations.CellPadding = TableCellPadding;
			operations.CellSpacing = TableCellSpacing;
			operations.CssClass = OperationTableCssClass;
			operations.ControlStyle.Width = Unit.Percentage(100);
			operations.Rows.Add(CreateOperationsHeader());
			foreach (MBeanOperationInfo operInfo in info.Operations)
			{
				OperationTableRow operationRow = new OperationTableRow(new ObjectName(ObjectName), operInfo, Proxy.ServerConnection, "Operation", ButtonCssClass);
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
			attrHeader.CssClass = AttributeTableCssClass;
			AddAttributesHeaderCell(attrHeader, Resources.MBeanUI.AttributesName, 20);
			AddAttributesHeaderCell(attrHeader, Resources.MBeanUI.AttributesDescription, 20);
			AddAttributesHeaderCell(attrHeader, Resources.MBeanUI.AttributesAccess, 10);
			AddAttributesHeaderCell(attrHeader, Resources.MBeanUI.AttributesValue, 25);
			AddAttributesHeaderCell(attrHeader, Resources.MBeanUI.AttributesActions, 25);
			return attrHeader;
		}
		private TableHeaderRow CreateOperationsHeader()
		{
			TableHeaderRow operHeader = new TableHeaderRow();
			operHeader.CssClass = OperationTableCssClass;
			AddOperationsHeaderCell(operHeader, Resources.MBeanUI.OperationsName, 20);
			AddOperationsHeaderCell(operHeader, Resources.MBeanUI.OperationsDescription, 20);
			AddOperationsHeaderCell(operHeader, Resources.MBeanUI.OperationsImpact, 10);
			AddOperationsHeaderCell(operHeader, Resources.MBeanUI.OperationsArguments, 35);
			AddOperationsHeaderCell(operHeader, Resources.MBeanUI.OperationsActions, 15);
			return operHeader;
		}
		private void AddOperationsHeaderCell(TableHeaderRow row, string name, double percentSize)
		{
			TableHeaderCell cell = new TableHeaderCell();
			cell.CssClass = OperationTableCssClass;
			cell.ControlStyle.Width = Unit.Percentage(percentSize);
			cell.Text = name;
			row.Cells.Add(cell);
		}
		private void AddAttributesHeaderCell(TableHeaderRow row, string name, double percentSize)
		{
			TableHeaderCell cell = new TableHeaderCell();
			cell.CssClass = AttributeTableCssClass;
			cell.ControlStyle.Width = Unit.Percentage(percentSize);
			cell.Text = name;
			row.Cells.Add(cell);
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
		#endregion
	}
}