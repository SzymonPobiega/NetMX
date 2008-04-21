#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using NetMX.OpenMBean;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;

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
         get { return (TabularType)base.RootType; }
      }
      protected new ITabularData RootValue
      {
         get { return (ITabularData)base.RootValue; }
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
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
         //_view.DataBind();
      }
      protected override Control ContentControl
      {
         get
         {
            _view = new GridView();
            _view.AutoGenerateColumns = false;
            CompositeType rowType = RootType.RowType;
            foreach (string columnName in rowType.KeySet)
            {
               DataControlField field =
                  CreateField(columnName, rowType.GetOpenType(columnName), RootType.IndexNames.Contains(columnName));
               _view.Columns.Add(field);
            }
            CommandField commandColumn = new CommandField();
            commandColumn.ShowEditButton = true;
            ObjectDataSource source = new ObjectDataSource();
            source.ID = "dataSource";
            source.SelectMethod = "Select";
            source.UpdateMethod = "Update";
            source.TypeName = typeof(TabularDataSource).AssemblyQualifiedName;
            source.ObjectCreating += new ObjectDataSourceObjectEventHandler(source_ObjectCreating);
            Controls.Add(source);
            _view.DataSourceID = "dataSource";
            _view.Columns.Add(commandColumn);
            _view.RowUpdating += new GridViewUpdateEventHandler(_view_RowUpdating);
            return _view;
         }
      }

      void source_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
      {
         e.ObjectInstance = new TabularDataSource(RootValue);
      }

      void _view_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
      {
         _view.EditIndex = -1;
      }
      void _view_RowUpdating(object sender, GridViewUpdateEventArgs e)
      {
         Dictionary<string, object> newValues = new Dictionary<string, object>();
         foreach (string key in e.NewValues.Keys)
         {
            newValues[key] = e.NewValues[key];
         }
         e.NewValues.Clear();
         e.NewValues["values"] = newValues;
         _view.EditIndex = -1;
      }
      void _view_RowEditing(object sender, GridViewEditEventArgs e)
      {
         _view.EditIndex = e.NewEditIndex;
      }      

      protected override object ExtractValue()
      {
         //TabularDataSupport result = new TabularDataSupport(RootType);
         //return result;
         return RootValue;
      }
      #endregion

      #region Utility
      private DataControlField CreateField(string columnName, OpenType columnType, bool keyColumn)
      {
         BoundField result = new TabularBoundField();
         result.ConvertEmptyStringToNull = true;
         result.DataField = columnName;
         result.ReadOnly = keyColumn;
         return result;
      }
      #endregion
   }

   public class TabularDataSource
   {
      private ITabularData _data;

      public TabularDataSource(ITabularData data)
      {
         _data = data;
      }

      public IEnumerable Select()
      {
         return _data.Values;
      }

      public void Update(Dictionary<string, object> values)
      {
         List<object> key = new List<object>();
         List<object> newValue = new List<object>();
         CompositeType rowType = _data.TabularType.RowType;
         foreach (string indexName in _data.TabularType.IndexNames)
         {
            TypeConverter conv = TypeDescriptor.GetConverter(((SimpleType)rowType.GetOpenType(indexName)).Representation);
            key.Add(conv.ConvertFromString((string)values[indexName]));
         }         
         _data.Remove(key);         
         foreach (string itemName in rowType.KeySet)
         {
            TypeConverter conv = TypeDescriptor.GetConverter(((SimpleType)rowType.GetOpenType(itemName)).Representation);
            newValue.Add(conv.ConvertFromString((string)values[itemName]));
         }
         CompositeDataSupport newRowValue = new CompositeDataSupport(rowType, rowType.KeySet, newValue);             
         _data.Put(newRowValue);
      }
   }

   public class TabularBoundField : BoundField
   {
      protected override object GetValue(Control controlContainer)
      {
         ICompositeData row = (ICompositeData)DataBinder.GetDataItem(controlContainer);
         return row[DataField];
      }
      public override void ExtractValuesFromCell(System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
      {
         Control control = null;
         string dataField = this.DataField;
         object text = null;
         string nullDisplayText = this.NullDisplayText;
         if (((rowState & DataControlRowState.Insert) == DataControlRowState.Normal) || this.InsertVisible)
         {
            if (cell.Controls.Count > 0)
            {
               control = cell.Controls[0];
               TextBox box = control as TextBox;
               if (box != null)
               {
                  text = box.Text;
               }
            }
            else 
            {
               string s = cell.Text;
               if (s == "&nbsp;")
               {
                  text = string.Empty;
               }
               else if (this.SupportsHtmlEncode && this.HtmlEncode)
               {
                  text = HttpUtility.HtmlDecode(s);
               }
               else
               {
                  text = s;
               }
            }
            if (text != null)
            {
               if (((text is string) && (((string)text).Length == 0)) && this.ConvertEmptyStringToNull)
               {
                  text = null;
               }
               if (((text is string) && (((string)text) == nullDisplayText)) && (nullDisplayText.Length > 0))
               {
                  text = null;
               }
               if (dictionary.Contains(dataField))
               {
                  dictionary[dataField] = text;
               }
               else
               {
                  dictionary.Add(dataField, text);
               }
            }
         }
      }
   }
}
