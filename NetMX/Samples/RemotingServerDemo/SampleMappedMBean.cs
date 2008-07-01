using System;
using System.Collections.Generic;
using System.Text;

namespace RemotingServerDemo
{
   public interface SampleMappedMBean
   {
      List<CollectionElement> Elements { get;  }
      List<CollectionElement> GetElementsByName(string name);
   }

   public class CollectionElement
   {
      private readonly List<NestedCollectionElement> _elements = new List<NestedCollectionElement>();
      public List<NestedCollectionElement> Elements
      {
         get { return _elements; }
      }
   }

   public class NestedCollectionElement
   {
      private readonly int _intValue;
      private readonly string _stringValue;

      public NestedCollectionElement(int intValue, string stringValue)
      {
         _intValue = intValue;
         _stringValue = stringValue;
      }

      public int IntValue
      {
         get { return _intValue; }
      }
      public string StringValue
      {
         get { return _stringValue; }
      }
   }

   public class SampleMapped : SampleMappedMBean
   {
      private readonly List<CollectionElement> _elements = new List<CollectionElement>();

      public void Add(CollectionElement element)
      {
         _elements.Add(element);
      }
      public List<CollectionElement> Elements
      {
         get { return _elements; }
      }
      public List<CollectionElement> GetElementsByName(string name)
      {
         List<CollectionElement> results = new List<CollectionElement>();
         foreach (CollectionElement element in _elements)
         {
            bool add = false;
            foreach (NestedCollectionElement collectionElement in element.Elements)
            {
               if (collectionElement.StringValue == name)
               {
                  add = true;
                  break;                  
               }
            }
            if (add)
            {
               results.Add(element);
            }
         }
         return results;
      }
   }
}
