#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using NetMX.OpenMBean;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace NetMX.WebUI.WebControls
{
   public class TabularValueControl : ComplexValueControlBase
   {
      #region MEMBERS
      private GridView _view;
      #endregion

      #region PROPERTIES
      protected new TabularType RootType
      {
         get { return (TabularType) base.RootType; }
      }
      protected new ITabularData RootValue
      {
         get { return (ITabularData) base.RootValue; }
      }
      #endregion

      #region CONSTRUCTOR
      public TabularValueControl()
      {
      }
      public TabularValueControl(bool editMode, TabularType rootType, ITabularData rootValue, OpenTypeIndex index)
         : base(editMode, rootType, rootValue, index)
      {
      }
      #endregion

      #region Overridden
      protected override Control ContentControl
      {
         get
         {
            _view = new GridView();
            _view.AutoGenerateColumns = false;
            CompositeType rowType = RootType.RowType;            
            foreach (string columnName in rowType.KeySet)
            {
               _view.Columns.Add(CreateField(columnName, rowType.GetOpenType(columnName)));
            }
            _view.Columns.Add(new CommandField());
            _view.DataSource = RootValue.Values;
            _view.RowUpdated += new GridViewUpdatedEventHandler(_view_RowUpdated);
            _view.DataBind();
            return _view;
         }
      }

      private void _view_RowUpdated(object sender, GridViewUpdatedEventArgs e)
      {         
      }
      
      protected override object ExtractValue()
      {         
         TabularDataSupport result = new TabularDataSupport(RootType);         
         return result;
      }
      #endregion

      #region Utility
      private  DataControlField CreateField(string columnName, OpenType columnType)
      {
         BoundField result = new TabularBoundField();         
         result.ConvertEmptyStringToNull = true;
         result.DataField = columnName;
         return result;
      }
      #endregion
   }
   public class TabularBoundField : BoundField
   {
      protected override object GetValue(Control controlContainer)
      {                  
         ICompositeData row = (ICompositeData) DataBinder.GetDataItem(controlContainer);
         return row[DataField];         
      }      
   }
}
