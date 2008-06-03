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
using System.Collections.Specialized;

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
      public TabularValueControl(bool editMode, TabularType rootType, ITabularData rootValue, OpenTypeIndex index, string title)
         : base(editMode, rootType, rootValue, index, title)
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
            _view.CssClass = Context.TabularDataTableCssClass;
            _view.RowStyle.CssClass = Context.TabularDataTableCssClass;
            _view.CellPadding = Context.TableCellPadding;
            _view.CellSpacing = Context.TableCellSpacing;
            
            CompositeType rowType = RootType.RowType;
            bool renderEditButton = false;
            foreach (string columnName in rowType.KeySet)
            {
               OpenType columnOpenType = rowType.GetOpenType(columnName);
               renderEditButton |= (columnOpenType.Kind == OpenTypeKind.SimpleType && !RootType.IndexNames.Contains(columnName));
               string columnDescription = rowType.GetDescription(columnName);
               DataControlField field =
                  CreateField(columnName, columnDescription, columnOpenType, RootType.IndexNames.Contains(columnName));
               field.ItemStyle.CssClass = Context.TabularDataTableCssClass;
               field.HeaderStyle.CssClass = Context.TabularDataTableCssClass;
               _view.Columns.Add(field);
            }            
            if (renderEditButton)
            {
               CommandField commandColumn = new CommandField();
               commandColumn.ShowEditButton = true;
               commandColumn.EditText = "Edit simple values";
               commandColumn.ItemStyle.CssClass = Context.TabularDataTableCssClass;
               commandColumn.HeaderStyle.CssClass = Context.TabularDataTableCssClass;
               _view.Columns.Add(commandColumn);
            }
            Controls.Add(CreateDataSource());
            _view.DataSourceID = "dataSource";            
            _view.RowUpdating += HandleRowUpdating;
            _view.RowCancelingEdit += HandleRowCancelingEdit;
            _view.RowEditing += HandleRowEditing;
            _view.RowCommand += HandleRowCommand;            
            return _view;
         }
      }

      private ObjectDataSource CreateDataSource()
      {
         ObjectDataSource source = new ObjectDataSource();
         source.ID = "dataSource";
         source.SelectMethod = "Select";
         source.UpdateMethod = "Update";
         source.TypeName = typeof(TabularDataSource).AssemblyQualifiedName;
         source.ObjectCreating += HandleObjectCreating;
         return source;
      }

      private void HandleRowCommand(object sender, GridViewCommandEventArgs e)
      {
         if (e.CommandName.StartsWith("EditNested"))
         {
            OrderedDictionary keyValues = new OrderedDictionary();
            foreach (DataControlFieldCell cell in _view.Rows[int.Parse((string) e.CommandArgument)].Cells)
            {
               BoundField boundField = cell.ContainingField as BoundField;
               if (boundField != null && RootType.IndexNames.Contains(boundField.DataField))
               {
                  boundField.ExtractValuesFromCell(keyValues, cell, DataControlRowState.Normal, true);
               }
            }
            List<object> key = new List<object>();
            foreach (string indexName in RootType.IndexNames)
            {
               TypeConverter conv =
                  TypeDescriptor.GetConverter(((SimpleType) RootType.RowType.GetOpenType(indexName)).Representation);
               key.Add(conv.ConvertFromString((string)keyValues[indexName]));
            }
            string[] parts = e.CommandName.Split('|');
            base.AddNestedControl(new TabularTypeIndex(key, parts[1]));
         }
      }

      private void HandleObjectCreating(object sender, ObjectDataSourceEventArgs e)
      {
         e.ObjectInstance = new TabularDataSource(RootValue);
      }

      private void HandleRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
      {
         SetButtonsState(true);
      }
      private void HandleRowUpdating(object sender, GridViewUpdateEventArgs e)
      {
         Dictionary<string, object> newValues = new Dictionary<string, object>();
         foreach (string key in e.NewValues.Keys)
         {
            newValues[key] = e.NewValues[key];
         }
         e.NewValues.Clear();
         e.NewValues["values"] = newValues;
         _view.EditIndex = -1;
         SetButtonsState(true);
      }
      private void HandleRowEditing(object sender, GridViewEditEventArgs e)
      {
         SetButtonsState(false);
      }      

      protected override object ExtractValue()
      {         
         return RootValue;
      }
      #endregion

      #region Utility
      private static DataControlField CreateField(string columnName, string columnDescription, OpenType columnType, bool keyColumn)
      {
         if (columnType.Kind != OpenTypeKind.SimpleType)
         {
            ButtonField result = new ButtonField();            
            result.CommandName = "EditNested|"+columnName;
            result.Text = "Edit";            
            result.HeaderText = columnDescription;
            return result;               
         }
         else
         {
            TabularBoundField result = new TabularBoundField();
            result.ConvertEmptyStringToNull = true;
            result.DataField = columnName;
            result.ReadOnly = keyColumn;
            result.HeaderText = columnDescription;
            return result;
         }
      }
      #endregion

      public class TabularBoundField : BoundField
      {
         protected override object GetValue(Control controlContainer)
         {
            ICompositeData row = (ICompositeData)DataBinder.GetDataItem(controlContainer);
            return row[DataField];
         }
         public override void ExtractValuesFromCell(System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
         {
            string dataField = this.DataField;
            object text = null;
            string nullDisplayText = this.NullDisplayText;
            if (((rowState & DataControlRowState.Insert) == DataControlRowState.Normal) || this.InsertVisible)
            {
               if (cell.Controls.Count > 0)
               {
                  Control control = cell.Controls[0];
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
}
