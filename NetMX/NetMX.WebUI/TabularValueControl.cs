#region USING
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using NetMX.OpenMBean;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
#endregion

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// Provides edit and view functionality for tabular data structures using standard <see cref="GridView"/> control.
   /// </summary>
   internal class TabularValueControl : ComplexValueControlBase
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
      /// <summary>
      /// Creates new <see cref="TabularValueControl"/> instance which reads its data from ControlState. 
      /// Used by <see cref="ComplexValueControlBase.Recreate"/> method.
      /// </summary>
      internal TabularValueControl()
      {
      }
      /// <summary>
      /// Creates new <see cref="TabularValueControl"/> instance providing all the necessary date in 
      /// constructor arguments.
      /// </summary>
      /// <param name="editMode">Will the control allow to edit the value, or only to view it.</param>
      /// <param name="rootType">Type of value.</param>
      /// <param name="rootValue">Value to edit or view.</param>
      /// <param name="index">An index within the root value.</param>
      /// <param name="title">Title of the control (provided only if control is in the bottom of the stack).</param>
      internal TabularValueControl(bool editMode, TabularType rootType, ITabularData rootValue, OpenTypeIndex index, string title)
         : base(editMode, rootType, rootValue, index, title)
      {
      }
      #endregion

      #region Overridden      
      protected override Control CreateContentControl()
      {
         _view = new GridView();
         _view.AutoGenerateColumns = false;
         _view.CssClass = UIContext.TabularDataTableCssClass;
         _view.RowStyle.CssClass = UIContext.TabularDataTableCssClass;
         _view.CellPadding = UIContext.TableCellPadding;
         _view.CellSpacing = UIContext.TableCellSpacing;

         CompositeType rowType = RootType.RowType;
         bool renderEditButton = false;
         foreach (string columnName in rowType.KeySet)
         {
            OpenType columnOpenType = rowType.GetOpenType(columnName);
            renderEditButton |= (columnOpenType.Kind == OpenTypeKind.SimpleType &&
                                 !RootType.IndexNames.Contains(columnName));
            string columnDescription = rowType.GetDescription(columnName);
            DataControlField field =
               CreateField(columnName, columnDescription, columnOpenType, RootType.IndexNames.Contains(columnName));
            field.ItemStyle.CssClass = UIContext.TabularDataTableCssClass;
            field.HeaderStyle.CssClass = UIContext.TabularDataTableCssClass;
            _view.Columns.Add(field);
         }
         if (renderEditButton)
         {
            CommandField commandColumn = new CommandField();
            commandColumn.ShowEditButton = true;
            commandColumn.EditText = "Edit simple values";
            commandColumn.ItemStyle.CssClass = UIContext.TabularDataTableCssClass;
            commandColumn.HeaderStyle.CssClass = UIContext.TabularDataTableCssClass;
            _view.Columns.Add(commandColumn);
         }
         Controls.Add(CreateDataSource());
         _view.DataSourceID = "dataSource";
         _view.RowUpdating += OnRowUpdating;
         _view.RowCancelingEdit += OnRowCancelingEdit;
         _view.RowEditing += OnRowEditing;
         _view.RowCommand += OnRowCommand;
         return _view;
      }
      protected override object ExtractValue()
      {         
         return RootValue;
      }
      #endregion

      #region Event handlers
      private void OnRowCommand(object sender, GridViewCommandEventArgs e)
      {
         if (e.CommandName.StartsWith("EditNested"))
         {
            OrderedDictionary keyValues = new OrderedDictionary();
            foreach (DataControlFieldCell cell in _view.Rows[int.Parse((string)e.CommandArgument)].Cells)
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
                  TypeDescriptor.GetConverter(RootType.RowType.GetOpenType(indexName).Representation);
               key.Add(conv.ConvertFromString((string)keyValues[indexName]));
            }
            string[] parts = e.CommandName.Split('|');
            AddNestedControl(new TabularTypeIndex(key, parts[1]));
         }
      }

      private void OnObjectCreating(object sender, ObjectDataSourceEventArgs e)
      {
         e.ObjectInstance = new TabularDataSource(RootValue);
      }

      private void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
      {
         SetButtonsState(true);
      }
      private void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
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
      private void OnRowEditing(object sender, GridViewEditEventArgs e)
      {
         SetButtonsState(false);
      }      
      #endregion

      #region Utility
      private ObjectDataSource CreateDataSource()
      {
         ObjectDataSource source = new ObjectDataSource();
         source.ID = "dataSource";
         source.SelectMethod = "Select";
         source.UpdateMethod = "Update";
         source.TypeName = typeof(TabularDataSource).AssemblyQualifiedName;
         source.ObjectCreating += OnObjectCreating;
         return source;
      }      
      private static DataControlField CreateField(string columnName, string columnDescription, OpenType columnType, bool keyColumn)
      {
         if (columnType.Kind != OpenTypeKind.SimpleType)
         {
            ButtonField result = new EditNestedValueButtonField();            
            result.CommandName = "EditNested|"+columnName;
            result.Text = "Edit";            
            result.HeaderText = columnDescription;
            return result;               
         }
         else
         {
            ComplexValueBoundField result = new ComplexValueBoundField(columnType);
            result.ConvertEmptyStringToNull = true;
            result.DataField = columnName;
            result.ReadOnly = keyColumn;
            result.HeaderText = columnDescription;
            return result;
         }
      }
      #endregion

      #region Button field
      private class EditNestedValueButtonField : ButtonField
      {
         public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
         {
            if (rowState != DataControlRowState.Edit)
            {
               base.InitializeCell(cell, cellType, rowState, rowIndex);
            }
         }
      }
      #endregion

      #region Data source class
      /// <summary>
      /// Used by <see cref="ObjectDataSource"/> to perform data query and manipulation operations. 
      /// </summary>
      private class TabularDataSource
      {
         private readonly ITabularData _data;

         /// <summary>
         /// Creates new <see cref="TabularDataSource"/> instance.
         /// </summary>
         /// <param name="data">Tabular value to perform operations on.</param>
         public TabularDataSource(ITabularData data)
         {
            _data = data;
         }

         /// <summary>
         /// Retrieves rows from the tabular value.
         /// </summary>
         /// <returns>Collection of rows as <see cref="ICompositeData"/>.</returns>
         public IEnumerable Select()
         {
            return _data.Values;
         }

         /// <summary>
         /// Updates the tabular value with values provied by user.
         /// </summary>
         /// <param name="values">Dictionary mapping row item names to their values.</param>
         public void Update(Dictionary<string, object> values)
         {
            List<object> key = new List<object>();
            List<object> newValue = new List<object>();
            CompositeType rowType = _data.TabularType.RowType;
            foreach (string indexName in _data.TabularType.IndexNames)
            {
               TypeConverter conv = TypeDescriptor.GetConverter(rowType.GetOpenType(indexName).Representation);
               key.Add(conv.ConvertFromString((string)values[indexName]));
            }
            ICompositeData existingValue = _data[key];
            _data.Remove(key);
            foreach (string itemName in rowType.KeySet)
            {
               if (values.ContainsKey(itemName))
               {
                  TypeConverter conv =
                     TypeDescriptor.GetConverter(rowType.GetOpenType(itemName).Representation);
                  newValue.Add(conv.ConvertFromString((string) values[itemName]));
               }
               else
               {
                  newValue.Add(existingValue[itemName]);
               }
            }
            CompositeDataSupport newRowValue = new CompositeDataSupport(rowType, rowType.KeySet, newValue);
            _data.Put(newRowValue);
         }
      }   
      #endregion
   }
   
}
