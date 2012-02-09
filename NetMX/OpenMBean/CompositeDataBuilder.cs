using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   public interface ICompositeDataBuilder
   {
      ICompositeDataBuilder Simple(string name, object value);
      ICompositeDataBuilder Composite(string name, Action<ICompositeDataBuilder> nestedCompositeValueBuilder);
      ICompositeDataBuilder Table(string name, Action<ITabularData> nestedTabularBuilder);
   }

   public class CompositeDataBuilder : ICompositeDataBuilder
   {
      private readonly CompositeType _type;
      private readonly Dictionary<string, object> _items = new Dictionary<string, object>();

      public CompositeDataBuilder(CompositeType type)
      {
         _type = type;
      }

      public ICompositeDataBuilder Simple(string name, object value)
      {
         if (name == null)
         {
            throw new ArgumentNullException("name");
         }
         _items.Add(name, value);
         return this;
      }

      public ICompositeDataBuilder Composite(string name, Action<ICompositeDataBuilder> nestedCompositeValueBuilderAction)
      {
         if (name == null)
         {
            throw new ArgumentNullException("name");
         }
         if (nestedCompositeValueBuilderAction == null)
         {
            throw new ArgumentNullException("nestedCompositeValueBuilderAction");
         }
         CompositeType nestedCompositeType = _type.GetOpenType(name) as CompositeType;
         if (nestedCompositeType == null)
         {
            throw new InvalidOperationException(string.Format("Item {0} is not of CompositeType", name));
         }
         CompositeDataBuilder nestedBuilder = new CompositeDataBuilder(nestedCompositeType);
         nestedCompositeValueBuilderAction(nestedBuilder);
         _items.Add(name, nestedBuilder.Create());
         return this;
      }

      public ICompositeDataBuilder Table(string name, Action<ITabularData> nestedTabularBuilder)
      {
         if (name == null)
         {
            throw new ArgumentNullException("name");
         }
         if (nestedTabularBuilder == null)
         {
            throw new ArgumentNullException("nestedTabularBuilder");
         }
         TabularType nestedTabularType = _type.GetOpenType(name) as TabularType;
         if (nestedTabularType == null)
         {
            throw new InvalidOperationException(string.Format("Item {0} is not of TabularType", name));
         }
         TabularDataSupport nestedTable = new TabularDataSupport(nestedTabularType);
         nestedTabularBuilder(nestedTable);
         _items.Add(name, nestedTable);
         return this;
      }

      public CompositeDataSupport Create()
      {
         return new CompositeDataSupport(_type, _items);
      }
   }
}