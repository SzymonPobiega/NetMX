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
   /// Provides edit and view functionality for composite data structures using standard <see cref="DetailsView"/> control.
   /// </summary>
   internal class CompositeValueControl : ComplexValueControlBase
   {
      #region MEMBERS
      private DetailsView _view;
      #endregion

      #region PROPERTIES
      protected new CompositeType RootType
      {
         get { return (CompositeType)base.RootType; }
      }
      protected new ICompositeData RootValue
      {
         get { return (ICompositeData)base.RootValue; }
         set { base.RootValue = value; }
      }
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates new <see cref="CompositeValueControl"/> instance which reads its data from ControlState. 
      /// Used by <see cref="ComplexValueControlBase.Recreate"/> method.
      /// </summary>
      internal CompositeValueControl()
      {
      }
      /// <summary>
      /// Creates new <see cref="CompositeValueControl"/> instance providing all the necessary date in 
      /// constructor arguments.
      /// </summary>
      /// <param name="editMode">Will the control allow to edit the value, or only to view it.</param>
      /// <param name="rootType">Type of value.</param>
      /// <param name="rootValue">Value to edit or view.</param>
      /// <param name="index">An index within the root value.</param>
      /// <param name="title">Title of the control (provided only if control is in the bottom of the stack).</param>
      internal CompositeValueControl(bool editMode, CompositeType rootType, ICompositeData rootValue, OpenTypeIndex index, string title)
         : base(editMode, rootType, rootValue, index, title)
      {
      }
      #endregion

      #region Overridden
      protected override Control CreateContentControl()
      {
         _view = new DetailsView();
         _view.AutoGenerateRows = false;
         _view.CssClass = UIContext.TabularDataTableCssClass;
         _view.RowStyle.CssClass = UIContext.TabularDataTableCssClass;
         _view.CellPadding = UIContext.TableCellPadding;
         _view.CellSpacing = UIContext.TableCellSpacing;
         _view.ChangeMode(DetailsViewMode.Edit);

         foreach (string itemName in RootType.KeySet)
         {
            OpenType columnOpenType = RootType.GetOpenType(itemName);
            string columnDescription = RootType.GetDescription(itemName);
            DataControlField field =
               CreateField(itemName, columnDescription, columnOpenType);
            field.ItemStyle.CssClass = UIContext.TabularDataTableCssClass;
            field.HeaderStyle.CssClass = UIContext.TabularDataTableCssClass;
            _view.Fields.Add(field);
         }
         Controls.Add(CreateDataSource());
         _view.DataSourceID = "dataSource";
         _view.ItemCommand += OnItemCommand;
         _view.ItemUpdating += OnItemUpdating;
         return _view;
      }
      protected override object ExtractValue()
      {
         return RootValue;
      }
      protected override void OnSubmit(object source, System.EventArgs e)
      {
         if (Page.IsValid)
         {
            _view.UpdateItem(true);
            base.OnSubmit(source, e);
         }
      }
      #endregion

      #region Event handlers
      private void OnItemCommand(object sender, DetailsViewCommandEventArgs e)
      {
         Page.Validate();         
         if (Page.IsValid && e.CommandName.StartsWith("EditNested"))
         {
            _view.UpdateItem(true);
            string[] parts = e.CommandName.Split('|');
            AddNestedControl(new CompositeTypeIndex(parts[1]));
         }
      }
      private void OnItemUpdating(object sender, DetailsViewUpdateEventArgs e)
      {
         Dictionary<string, object> newValues = new Dictionary<string, object>();
         foreach (string key in e.NewValues.Keys)
         {
            newValues[key] = e.NewValues[key];
         }
         e.NewValues.Clear();
         e.NewValues["values"] = newValues;
      }
      private void OnObjectCreating(object sender, ObjectDataSourceEventArgs e)
      {
         e.ObjectInstance = new CompositeDataSource(RootValue, delegate(ICompositeData newRootValue)
         {
            RootValue = newRootValue;
         });
      }
      #endregion

      #region Utility
      private ObjectDataSource CreateDataSource()
      {
         ObjectDataSource source = new ObjectDataSource();
         source.ID = "dataSource";
         source.SelectMethod = "Select";
         source.UpdateMethod = "Update";
         source.TypeName = typeof(CompositeDataSource).AssemblyQualifiedName;
         source.ObjectCreating += OnObjectCreating;
         return source;
      }
      private static DataControlField CreateField(string itemName, string itemDescription, OpenType itemType)
      {
         if (itemType.Kind != OpenTypeKind.SimpleType)
         {
            ButtonField result = new ButtonField();
            result.CommandName = "EditNested|" + itemName;
            result.Text = "Edit";            
            result.HeaderText = itemDescription;
            return result;
         }
         else
         {
            ComplexValueBoundField result = new ComplexValueBoundField(itemType);
            result.ConvertEmptyStringToNull = true;
            result.DataField = itemName;
            result.ReadOnly = false;            
            result.HeaderText = itemDescription;
            return result;
         }
      }
      #endregion

      //#region Button control
      //private class EditNestedValueButtonField : ButtonField
      //{
      //   override ins
      //}
      //#endregion

      #region Data source class
      private delegate void UpdateRootValueDelegate(ICompositeData newRootValue);
      /// <summary>
      /// Used by <see cref="ObjectDataSource"/> to perform data query and manipulation operations. 
      /// </summary>
      private class CompositeDataSource
      {
         private readonly ICompositeData _data;
         private readonly UpdateRootValueDelegate _updateRootValue;

         /// <summary>
         /// Creates new <see cref="CompositeDataSource"/> instance.
         /// </summary>
         /// <param name="data">Composite value to perform operations on.</param>
         /// <param name="updateRootValue">Callback method to call when updating value.</param>
         public CompositeDataSource(ICompositeData data, UpdateRootValueDelegate updateRootValue)
         {
            _data = data;
            _updateRootValue = updateRootValue;
         }

         /// <summary>
         /// Retrieves items from the composite value.
         /// </summary>
         /// <returns>Collection of values.</returns>
         public object Select()
         {
            return _data;
         }

         /// <summary>
         /// Updates the tabular value with values provied by user.
         /// </summary>
         /// <param name="values">Dictionary mapping item names to their values.</param>
         public void Update(Dictionary<string, object> values)
         {
            Dictionary<string, object> newItems = new Dictionary<string, object>();
            foreach (string itemName in _data.CompositeType.KeySet)
            {
               if (values.ContainsKey(itemName))
               {
                  TypeConverter conv = TypeDescriptor.GetConverter(_data.CompositeType.GetOpenType(itemName).Representation);
                  newItems[itemName] = conv.ConvertFromString((string)values[itemName]);
               }
               else
               {
                  newItems[itemName] = _data[itemName];
               }
            }
            ICompositeData newRootValue = new CompositeDataSupport(_data.CompositeType, newItems);
            _updateRootValue(newRootValue);
         }
      }
      #endregion
   }

}
