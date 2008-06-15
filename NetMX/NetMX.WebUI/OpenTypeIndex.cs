#region USING
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetMX.OpenMBean;
using System.Globalization;
#endregion

namespace NetMX.WebUI.WebControls
{
   [Serializable]
   internal abstract class OpenTypeIndex
   {      
      public abstract void UpdateValue(OpenType rootType, ref object rootValue, object value);
      public abstract string Visualize();

      public ComplexValueControlBase CreateControl(bool editMode, OpenType rootType, object rootValue)
      {
         OpenType nestedType;
         object nestedValue;
         ExtractNestedData(rootType, rootValue, out nestedType, out nestedValue);

         return DelegatingOpenTypeVisitor<ComplexValueControlBase>.VisitOpenType(nestedType, null, null,
         delegate(TabularType visited)
         {
            return new TabularValueControl(editMode, visited, (ITabularData)nestedValue, this, null);
         },
         delegate(CompositeType visited)
         {
            return new CompositeValueControl(editMode, visited, (ICompositeData)nestedValue, this, null);
         });
      }
      protected abstract void ExtractNestedData(OpenType rootType, object rootValue, out OpenType nestedType,
                                             out object nestedValue);

   }
   [Serializable]
   internal sealed class CompositeTypeIndex : OpenTypeIndex
   {
      private readonly string _itemName;

      public CompositeTypeIndex(string itemName)
      {
         _itemName = itemName;
      }

      protected override void ExtractNestedData(OpenType rootType, object rootValue, out OpenType nestedType,
                                             out object nestedValue)
      {
         nestedValue = null;
         CompositeType compositeRootType = (CompositeType)rootType;
         nestedType = compositeRootType.GetOpenType(_itemName);
         if (rootValue != null)
         {
            ICompositeData compositeData = (ICompositeData)rootValue;
            nestedValue = compositeData[_itemName];
         }
      }
      
      public override void UpdateValue(OpenType rootType, ref object rootValue, object value)
      {
         ICompositeData compositeData = (ICompositeData)rootValue;                           
         Dictionary<string, object> newItems = new Dictionary<string, object>();
         foreach (string itemName in compositeData.CompositeType.KeySet)
         {
            newItems[itemName] = _itemName == itemName ? value : compositeData[itemName];
         }
         rootValue = new CompositeDataSupport(compositeData.CompositeType, newItems);
      }

      public override string Visualize()
      {
         return string.Format(CultureInfo.CurrentCulture, "Item name: {0}", _itemName);
      }
   }
   [Serializable]
   internal sealed class TabularTypeIndex : OpenTypeIndex
   {
      private readonly ReadOnlyCollection<object> _rowKey;
      private readonly string _itemName;

      public TabularTypeIndex(IEnumerable<object> rowKey, string itemName)
      {
         _rowKey = new List<object>(rowKey).AsReadOnly();
         _itemName = itemName;
      }

      protected override void ExtractNestedData(OpenType rootType, object rootValue, out OpenType nestedType,
                                             out object nestedValue)
      {
         nestedValue = null;
         TabularType tabularRootType = (TabularType)rootType;
         nestedType = tabularRootType.RowType.GetOpenType(_itemName);
         if (rootValue != null)
         {
            ITabularData tabularData = (ITabularData)rootValue;
            ICompositeData row = tabularData[_rowKey];
            nestedValue = row[_itemName];
         }
      }
      
      public override void UpdateValue(OpenType rootType, ref object rootValue, object value)
      {
         ITabularData tabularData = (ITabularData)rootValue;
         ICompositeData row = tabularData[_rowKey];
         List<object> newValues = new List<object>();
         List<string> newKeys = new List<string>();
         foreach (string itemName in row.CompositeType.KeySet)
         {
            newKeys.Add(itemName);
            newValues.Add(_itemName == itemName ? value : row[itemName]);
         }
         tabularData.Remove(_rowKey);
         tabularData.Put(new CompositeDataSupport(row.CompositeType, newKeys, newValues));
      }

      public override string Visualize()
      {
         string[] keyStrings = new string[_rowKey.Count];
         for (int i = 0; i < keyStrings.Length; i++)
         {
            keyStrings[i] = _rowKey[i].ToString();
         }
         return string.Format(CultureInfo.CurrentCulture, "Row key: ({0}), item name: {1}", string.Join(", ", keyStrings), _itemName);
      }
   }
}
