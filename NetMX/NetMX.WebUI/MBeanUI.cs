using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetMX;
using System.ComponentModel;
using NetMX.OpenMBean;
using NetMX.Relation;
using System.Collections.Generic;
using NetMX.Proxy;

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// Provides the edit and view capabilities for MBeans.
   /// </summary>
   public class MBeanUI : CompositeControl
   {
      #region Members
      private bool _recreateOpenTypeView;
      private OpenTypeKind _openTypeKind;
      private object _activeFeatureSelector;
      #endregion

      #region Controls
      private PlaceHolder _beanView;
      private PlaceHolder _openValueView;
      private Button _refreshButton;
      private readonly Dictionary<object, IMBeanFeatureControl> _featureControls = new Dictionary<object, IMBeanFeatureControl>();
      #endregion

      #region Data source properties
      private string _mBeanServerProxyID;
      /// <summary>
      /// Gtes or sets ID of MBeanServerProxy control.
      /// </summary>
      [
      Category("Data source"),
      DefaultValue("")
      ]
      public string MBeanServerProxyID
      {
         get { return _mBeanServerProxyID; }
         set { _mBeanServerProxyID = value; }
      }
      private ObjectName _objectName;
      /// <summary>
      /// ObjectName of displayed/edited MBean
      /// </summary>
      public string ObjectName
      {
         get
         {
            return _objectName;
         }
         set
         {
            _objectName = value;
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
      /// <summary>
      /// Cell-padding of rendered tables (general info, attributes and operations)
      /// </summary>
      [
      Category("Appearance"),
      DefaultValue(0)
      ]
      public int TableCellPadding
      {
         get
         {
            return ViewStateExtensions.TryGetValue(ViewState, "TableCellPadding", 0);
         }
         set { ViewState["TableCellPadding"] = value; }
      }
      /// <summary>
      /// Gets or sets the CSS class associated with HTML table presenting MBean attributes. 
      /// It is applied to the TABLE, TR and TD elements.
      /// </summary>
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string AttributeTableCssClass
      {
         get
         {
            return ViewStateExtensions.TryGetValue<string>(ViewState, "AttributeTableCssClass");
         }
         set { ViewState["AttributeTableCssClass"] = value; }
      }
      /// <summary>
      /// Gets or sets the CSS class associated with HTML table presenting MBean tabular data attributes. 
      /// It is applied to the TABLE, TR and TD elements.
      /// </summary>
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string TabularDataTableCssClass
      {
         get 
         {
            return ViewStateExtensions.TryGetValue<string>(ViewState, "TabularDataTableCssClass");
         }
         set { ViewState["TabularDataTableCssClass"] = value; }
      }
      /// <summary>
      /// <summary>
      /// Gets or sets the CSS class associated with HTML table presenting MBean operations. 
      /// It is applied to the TABLE, TR and TD elements.
      /// </summary>
      [
      Category("Appearance"),
      DefaultValue("")
      ]
      public string OperationTableCssClass
      {
         get
         {
            return ViewStateExtensions.TryGetValue<string>(ViewState, "OperationTableCssClass");
         }
         set { ViewState["OperationTableCssClass"] = value; }
      }
      private string _relationTableCssClass;
      ///<summary>
      /// Gets or sets the CSS class associated with HTML table presenting MBean relations. 
      /// It is applied to the TABLE, TR and TD elements.
      ///</summary>
      [Category("Appearance"),DefaultValue("")]
      public string RelationTableCssClass
      {
         get { return _relationTableCssClass; }
         set { _relationTableCssClass = value; }
      }
      private string _generalInfoCssClass;
      ///<summary>
      /// Gets or sets the CSS class associated with HTML table presenting MBean general info. 
      /// It is applied to the TABLE element only.
      ///</summary>
      [Category("Appearance"),DefaultValue("")]
      public string GeneralInfoCssClass
      {
         get { return _generalInfoCssClass; }
         set { _generalInfoCssClass = value; }
      }
      private string _generalInfoNameCssClass;
      ///<summary>
      /// Gets or sets the CSS class associated with HTML cells in the MBean general info "Name" column.       
      ///</summary>
      [Category("Appearance"),DefaultValue("")]
      public string GeneralInfoNameCssClass
      {
         get { return _generalInfoNameCssClass; }
         set { _generalInfoNameCssClass = value; }
      }
      private string _generalInfoValueCssClass;
      ///<summary>
      /// Gets or sets the CSS class associated with HTML cells in the MBean general info "Name" column.       
      ///</summary>
      [Category("Appearance"),DefaultValue("")]
      public string GeneralInfoValueCssClass
      {
         get { return _generalInfoValueCssClass; }
         set { _generalInfoValueCssClass = value; }
      }
      private string _sectionTitleCssClass;
      /// <summary>
      /// Gets of sets the CSS class associated with "Title" HTML element.
      /// </summary>
      [Category("Appearance"),DefaultValue("")]
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
      protected override void OnInit(EventArgs e)
      {
         base.OnInit(e);
         Page.RegisterRequiresControlState(this);
      }
      protected override void LoadControlState(object savedState)
      {
         object[] state = (object[])savedState;
         base.LoadControlState(state[0]);
         _recreateOpenTypeView = (bool)state[1];
         _openTypeKind = (OpenTypeKind)state[2];
         _objectName = (ObjectName)state[3];
         _activeFeatureSelector = state[4];
      }
      protected override object SaveControlState()
      {
         return new object[] { base.SaveControlState(), _recreateOpenTypeView, _openTypeKind, _objectName, _activeFeatureSelector };
      }
      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         if (ObjectName != null)
         {
            CreateControls();
         }
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
         _openValueView.Visible = _recreateOpenTypeView;
         _beanView.Visible = _refreshButton.Visible = !_recreateOpenTypeView;
      }
      #endregion

      #region Event handlers
      private void NavigateCommand(object sender, CommandEventArgs e)
      {
         ObjectName = new ObjectName((string)e.CommandArgument);
         Controls.Clear();
         CreateControls();
      }
      private void HandleViewEditOpenType(object sender, ViewEditOpenTypeEventArgs e)
      {
         ComplexValueControlBase ctl = ComplexValueControlBase.Create(e.EnableEdit, e.Type, e.Value, e.Description);
         ctl.ID = "openTypeViewControl";
         ctl.Cancel += HandleCancelOpenType;
         _openValueView.Controls.Add(ctl);
         _openTypeKind = e.Type.Kind;
         _recreateOpenTypeView = true;
         _activeFeatureSelector = e.Selector;
      }
      private void HandleChangeUIState(object sender, ChangeUIStateEventArgs e)
      {
         foreach (object selector in _featureControls.Keys)
         {
            if (!e.Selector.Equals(selector))
            {
               _featureControls[selector].SetUIState(e.UIState);
            }
         }
      }
      private void HandleCancelOpenType(object sender, EventArgs e)
      {
         _recreateOpenTypeView = false;
         _activeFeatureSelector = null;
      }
      private void HandleSubmitOpenType(object sender, ValueAndIndexEventArgs e)
      {
         _featureControls[_activeFeatureSelector].SetOpenTypeValue(_activeFeatureSelector, e.Value);         
         _recreateOpenTypeView = false;
         _activeFeatureSelector = null;
      }
      #endregion

      #region Utility
      private void CreateControls()
      {
         _beanView = new PlaceHolder();
         _beanView.EnableViewState = false;
         _beanView.ID = "beanView";
         _openValueView = new PlaceHolder();
         _openValueView.EnableViewState = false;
         _openValueView.ID = "openValueView";

         if (_recreateOpenTypeView)
         {
            ComplexValueControlBase ctl = ComplexValueControlBase.Recreate(_openTypeKind);
            ctl.ID = "openTypeViewControl";
            ctl.Cancel += HandleCancelOpenType;
            ctl.Submit += HandleSubmitOpenType;
            _openValueView.Controls.Add(ctl);
         }

         MBeanInfo info = Proxy.ServerConnection.GetMBeanInfo(new ObjectName(ObjectName));
         RelationServiceMBean relationService = null;
         if (Proxy.ServerConnection.IsRegistered(RelationService.ObjectName))
         {
            relationService = NetMXProxyExtensions.NewMBeanProxy<RelationServiceMBean>(Proxy.ServerConnection,
                                                                        RelationService.ObjectName);
         }

         Label generalInfoTitle = new Label();
         generalInfoTitle.Text = Resources.MBeanUI.GeneralInformationSection + "&nbsp;&nbsp;";
         generalInfoTitle.CssClass = SectionTitleCssClass;
         Controls.Add(generalInfoTitle);

         _refreshButton = new Button();
         _refreshButton.Text = Resources.MBeanUI.RefreshButton;
         _refreshButton.CssClass = ButtonCssClass;
         Controls.Add(_refreshButton);

         Table generalInfo = new Table();
         generalInfo.CellPadding = TableCellPadding;
         generalInfo.CellSpacing = TableCellSpacing;
         generalInfo.CssClass = GeneralInfoCssClass;
         generalInfo.ControlStyle.Width = Unit.Percentage(100);
         AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationObjectName, ObjectName);
         AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationDescription, info.Description);
         AddGeneralInfoItem(generalInfo, Resources.MBeanUI.GeneralInformationClass, info.ClassName);
         Controls.Add(generalInfo);

         Controls.Add(_beanView);
         Controls.Add(_openValueView);

         Label attributesTitle = new Label();
         attributesTitle.Text = Resources.MBeanUI.AttributesSection;
         attributesTitle.CssClass = SectionTitleCssClass;
         _beanView.Controls.Add(attributesTitle);

         Table attributes = new Table();
         attributes.ID = "attributes";
         attributes.CellPadding = TableCellPadding;
         attributes.CellSpacing = TableCellSpacing;
         attributes.CssClass = AttributeTableCssClass;
         attributes.ControlStyle.Width = Unit.Percentage(100);
         attributes.Rows.Add(CreateAttributesHeader());
         foreach (MBeanAttributeInfo attrInfo in info.Attributes)
         {
            AttributeTableRow attributeRow = new AttributeTableRow(new ObjectName(ObjectName), attrInfo, Proxy.ServerConnection);
            attributeRow.ViewEditOpenType += HandleViewEditOpenType;
            attributeRow.ChangeUIState += HandleChangeUIState;
            attributes.Rows.Add(attributeRow);
            _featureControls[attributeRow.Selector] = attributeRow;
         }
         _beanView.Controls.Add(attributes);

         Label operationTitle = new Label();
         operationTitle.Text = Resources.MBeanUI.OperationsSection;
         operationTitle.CssClass = SectionTitleCssClass;
         _beanView.Controls.Add(operationTitle);

         Table operations = new Table();
         operations.ID = "operations";
         operations.CellPadding = TableCellPadding;
         operations.CellSpacing = TableCellSpacing;
         operations.CssClass = OperationTableCssClass;
         operations.ControlStyle.Width = Unit.Percentage(100);
         operations.Rows.Add(CreateOperationsHeader());
         foreach (MBeanOperationInfo operInfo in info.Operations)
         {
            OperationTableRow operationRow = new OperationTableRow(new ObjectName(ObjectName), operInfo, Proxy.ServerConnection);
            operationRow.ViewEditOpenType += HandleViewEditOpenType;
            operationRow.ChangeUIState += HandleChangeUIState;
            _featureControls[operationRow.Selector] = operationRow;
            operations.Rows.Add(operationRow);            
         }
         _beanView.Controls.Add(operations);

         if (relationService != null)
         {
            Label relationsTitle = new Label();
            relationsTitle.Text = Resources.MBeanUI.RelationsSection;
            relationsTitle.CssClass = SectionTitleCssClass;
            _beanView.Controls.Add(relationsTitle);

            Table relations = new Table();
            relations.CellPadding = TableCellPadding;
            relations.CellSpacing = TableCellSpacing;
            relations.CssClass = RelationTableCssClass;
            relations.ControlStyle.Width = Unit.Percentage(100);
            relations.Rows.Add(CreateRelationsHeader());
            IDictionary<string, IList<string>> referencing = relationService.FindReferencingRelations(ObjectName, null,
                                                                                                      null);
            foreach (string relationId in referencing.Keys)
            {
               string relationType = relationService.GetRelationTypeName(relationId);
               IList<RoleInfo> roleInfos = relationService.GetRoleInfos(relationType);
               foreach (RoleInfo roleInfo in roleInfos)
               {
                  RelationRoleTableRow relationRow = new RelationRoleTableRow(ObjectName, relationId, roleInfo,
                                                                              relationService, RelationTableCssClass,
                                                                              NavigateCommand);
                  if (relationRow.HasValue)
                  {
                     relations.Rows.Add(relationRow);
                  }
               }
            }
            _beanView.Controls.Add(relations);
         }
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
               Control container = NamingContainer;
               while (container != null)
               {
                  _proxy = (MBeanServerProxy)container.FindControl(MBeanServerProxyID);
                  if (_proxy != null)
                  {
                     break;
                  }
                  container = container.NamingContainer;
               }
            }
            return _proxy;
         }
      }
      #endregion
   }
}