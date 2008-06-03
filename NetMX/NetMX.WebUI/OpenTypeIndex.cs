#region USING
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NetMX.OpenMBean;
using System.Globalization;
#endregion

namespace NetMX.WebUI.WebControls
{
   [Serializable]
   public abstract class OpenTypeIndex
   {
      public abstract ComplexValueControlBase CreateControl(bool editMode, OpenType rootType, object rootValue);
      public abstract void UpdateValue(OpenType rootType, object rootValue, object value);
      public abstract string Visualize();
   }
   [Serializable]
   public sealed class TabularTypeIndex : OpenTypeIndex
   {
      private ReadOnlyCollection<object> _rowKey;
      private string _itemName;

      public TabularTypeIndex(IEnumerable<object> rowKey, string itemName)
      {
         _rowKey = new List<object>(rowKey).AsReadOnly();
         _itemName = itemName;
      }

      public override ComplexValueControlBase CreateControl(bool editMode, OpenType rootType, object rootValue)
      {
         object nestedValue = null;
         TabularType tabularRootType = (TabularType)rootType;
         OpenType nestedType = tabularRootType.RowType.GetOpenType(_itemName);
         if (rootValue != null)
         {
            ITabularData tabularData = (ITabularData)rootValue;
            ICompositeData row = tabularData[_rowKey];
            nestedValue = row[_itemName];
         }
         return DelegatingOpenTypeVisitor<ComplexValueControlBase>.VisitOpenType(nestedType, null, null,
            delegate(TabularType visited)
            {
               return new TabularValueControl(editMode, visited, (ITabularData)nestedValue, this, null);
            },
               delegate(CompositeType visited)
               {
                  return null;
               });
      }
      public override void UpdateValue(OpenType rootType, object rootValue, object value)
      {
         TabularType tabularRootType = (TabularType)rootType;
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
