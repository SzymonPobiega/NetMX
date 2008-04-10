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
using NetMX.Relation;
using System.Collections.Generic;

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
      private string _relationTableCssClass;
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string RelationTableCssClass
      {
         get { return _relationTableCssClass; }
         set { _relationTableCssClass = value; }
      }
      private string _generalInfoCssClass;
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string GeneralInfoCssClass
      {
         get { return _generalInfoCssClass; }
         set { _generalInfoCssClass = value; }
      }
      private string _generalInfoNameCssClass;
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string GeneralInfoNameCssClass
      {
         get { return _generalInfoNameCssClass; }
         set { _generalInfoNameCssClass = value; }
      }
      private string _generalInfoValueCssClass;
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string GeneralInfoValueCssClass
      {
         get { return _generalInfoValueCssClass; }
         set { _generalInfoValueCssClass = value; }
      }
      private string _sectionTitleCssClass;
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string SectionTitleCssClass
      {
         get { return _sectionTitleCssClass; }
         set { _sectionTitleCssClass = value; }
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

      #region Event handlers
      private void NavigateCommand(object sender, CommandEventArgs e)
      {
         ObjectName = new ObjectName((string)e.CommandArgument);
         this.Controls.Clear();
         CreateControls();
      }
      #endregion

      #region Utility
      private void CreateControls()
		{
			MBeanInfo info = Proxy.ServerConnection.GetMBeanInfo(new ObjectName(ObjectName));
         RelationServiceMBean relationService = NetMX.NewMBeanProxy<RelationServiceMBean>(Proxy.ServerConnection, RelationService.ObjectName);

			Label generalInfoTitle = new Label();
			generalInfoTitle.Text = Resources.MBeanUI.GeneralInformationSection + "&nbsp;&nbsp;";
         generalInfoTitle.CssClass = SectionTitleCssClass;
			this.Controls.Add(generalInfoTitle);

			Button refreshButton = new Button();
			refreshButton.Text = Resources.MBeanUI.RefreshButton;
			refreshButton.CssClass = ButtonCssClass;
			this.Controls.Add(refreshButton);

			Table generalInfo = new Table();
			generalInfo.CellPadding = TableCellPadding;
			generalInfo.CellSpacing = TableCellSpacing;
         generalInfo.CssClass = GeneralInfoCssClass;
			generalInfo.ControlStyle.Width = Unit.Percentage(100);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationObjectName, ObjectName);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationDescription, info.Description);
			AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationClass, info.ClassName);
			this.Controls.Add(generalInfo);

			Label attributesTitle = new Label();
			attributesTitle.Text = Resources.MBeanUI.AttributesSection;
         attributesTitle.CssClass = SectionTitleCssClass;
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
         operationTitle.CssClass = SectionTitleCssClass;
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

         Label relationsTitle = new Label();
         relationsTitle.Text = Resources.MBeanUI.RelationsSection;
         relationsTitle.CssClass = SectionTitleCssClass;
         this.Controls.Add(relationsTitle);

         Table relations = new Table();
         relations.CellPadding = TableCellPadding;
         relations.CellSpacing = TableCellSpacing;
         relations.CssClass = RelationTableCssClass;
         relations.ControlStyle.Width = Unit.Percentage(100);
         relations.Rows.Add(CreateRelationsHeader());
         IDictionary<string, IList<string>> referencing = relationService.FindReferencingRelations(ObjectName, null, null);
         foreach (string relationId in referencing.Keys)
         {
            string relationType = relationService.GetRelationTypeName(relationId);
            IList<RoleInfo> roleInfos = relationService.GetRoleInfos(relationType);
            foreach (RoleInfo roleInfo in roleInfos)
            {
               RelationRoleTableRow relationRow = new RelationRoleTableRow(ObjectName, relationId, roleInfo, relationService, "Operation", this.NavigateCommand);
               if (relationRow.HasValue)
               {
                  relations.Rows.Add(relationRow);
               }
            }
         }          
         this.Controls.Add(relations);
		}
		private void AddGeneralInfoItem(Table table, string name, string value)
		{
			TableRow row = new TableRow();
         row.CssClass = GeneralInfoCssClass;

			TableCell nameCell = new TableCell();
         nameCell.CssClass = GeneralInfoNameCssClass;
			nameCell.Text = name;
			nameCell.ControlStyle.Width = Unit.Percentage(20);
			row.Cells.Add(nameCell);

			TableCell valueCell = new TableCell();
         valueCell.CssClass = GeneralInfoValueCssClass;
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
      private TableHeaderRow CreateRelationsHeader()
      {
         TableHeaderRow relHeader = new TableHeaderRow();
         relHeader.CssClass = RelationTableCssClass;
         AddRelationsHeaderCell(relHeader, Resources.MBeanUI.RelationsName, 20);
         AddRelationsHeaderCell(relHeader, Resources.MBeanUI.RelationsDescription, 20);
         AddRelationsHeaderCell(relHeader, Resources.MBeanUI.RelationsAccess, 10);
         AddRelationsHeaderCell(relHeader, Resources.MBeanUI.RelationsValue, 50);
         return relHeader;
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
      private void AddHeaderCell(TableHeaderRow row, string name, double percentSize, string cssClass)
      {
         TableHeaderCell cell = new TableHeaderCell();
         cell.CssClass = cssClass;
         cell.ControlStyle.Width = Unit.Percentage(percentSize);
         cell.Text = name;
         row.Cells.Add(cell);
      }
      private void AddRelationsHeaderCell(TableHeaderRow row, string name, double percentSize)
      {
         AddHeaderCell(row, name, percentSize, RelationTableCssClass);
      }
		private void AddOperationsHeaderCell(TableHeaderRow row, string name, double percentSize)
		{
         AddHeaderCell(row, name, percentSize, OperationTableCssClass);
		}      
		private void AddAttributesHeaderCell(TableHeaderRow row, string name, double percentSize)
		{
         AddHeaderCell(row, name, percentSize, AttributeTableCssClass);
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