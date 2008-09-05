using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// 
   /// </summary>
   [Serializable]
   public sealed class EnumerationType : OpenType
   {
      #region Fields
      private readonly ReadOnlyCollection<KeyValuePair<string, int>> _legalValues;
      #endregion

      #region Constructors
      /// <summary>
      /// Creates new <see cref="EnumerationType"/> instance.
      /// </summary>
      /// <param name="enumType"></param>      
      public EnumerationType(Type enumType)
         : base(typeof(int), enumType.AssemblyQualifiedName, GetDescription(enumType))
      {
         List<KeyValuePair<string, int>> legalValues = new List<KeyValuePair<string, int>>();
         foreach (object value in Enum.GetValues(enumType))
         {
            int intValue = Convert.ToInt32(value);
            string name = Enum.GetName(enumType, value);
            legalValues.Add(new KeyValuePair<string, int>(name, intValue));
         }
         _legalValues = legalValues.AsReadOnly();
      }
      /// <summary>
      /// Creates new <see cref="EnumerationType"/> instance.
      /// </summary>
      /// <param name="qualifiedTypeName"></param>
      /// <param name="description"></param>
      /// <param name="itemValues"></param>
      /// <param name="itemNames"></param>
      public EnumerationType(string qualifiedTypeName, string description, IEnumerable<int> itemValues, IEnumerable<string> itemNames)
         : base(typeof(int), qualifiedTypeName, description)
      {
         if (itemValues == null)
         {
            throw new ArgumentNullException("itemValues");
         }
         if (itemNames == null)
         {
            throw new ArgumentNullException("itemNames");
         }
         Dictionary<string, int> legalValues = new Dictionary<string, int>();

         IEnumerator<int> values = itemValues.GetEnumerator();
         foreach (string name in itemNames)
         {
            values.MoveNext();
            if (string.IsNullOrEmpty(name))
            {
               throw new ArgumentNullException("itemNames", "Item names cannot contain null or empty string items.");
            }
            if (legalValues.ContainsKey(name))
            {
               throw new OpenDataException("EnumerationType items cannot have duplicate names.");
            }
            legalValues[name] = values.Current;            
         }
         _legalValues = new List<KeyValuePair<string, int>>(legalValues).AsReadOnly();
      }
      #endregion

      #region Interface
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public IEnumerable<KeyValuePair<string, int>> GetLegalValues()
      {
         return _legalValues;
      }
      /// <summary>
      /// Gets the value associated with provided name.
      /// </summary>
      /// <param name="name">Name of the value.</param>
      /// <returns></returns>
      /// <exception cref="ArgumentOutOfRangeException">If provided name is not associated with legal value.</exception>
      public int GetValue(string name)
      {
         foreach (KeyValuePair<string, int> pair in _legalValues)
         {
            if (pair.Key == name)
            {
               return pair.Value;
            }
         }
         throw new ArgumentOutOfRangeException("name", string.Format(CultureInfo.CurrentCulture, @"Provied name (""{0}"") is not associated with any legal value.",name));
      }      
      /// <summary>
      /// Gets the name for a value.
      /// </summary>
      /// <param name="value">Value</param>
      /// <returns></returns>
      /// <exception cref="ArgumentOutOfRangeException">If provied value is not legal for this enumeration.</exception>
      public string GetName(int value)
      {
         foreach (KeyValuePair<string, int> pair in _legalValues)
         {
            if (pair.Value == value)
            {
               return pair.Key;
            }
         }
         throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, @"""{0}"" is not a legal value for this enumeration type.", value));
      }
      /// <summary>
      /// Checks if provided name is associated with any legal value of this enumeration.
      /// </summary>
      /// <param name="name">The name to be checked.</param>
      /// <returns>True if the name is associated with a legal value. False otherwise.</returns>
      public bool HasValue(string name)
      {
         foreach (KeyValuePair<string, int> pair in _legalValues)
         {
            if (pair.Key == name)
            {
               return true;
            }
         }
         return false;
      }
      /// <summary>
      /// Checks if provied value is legal for this enumaration.
      /// </summary>
      /// <param name="value">Value to be checked.</param>
      /// <returns>True if the value is legal. False otherwise.</returns>
      public bool HasValue(int value)
      {
         foreach (KeyValuePair<string, int> pair in _legalValues)
         {
            if (pair.Value == value)
            {
               return true;
            }
         }
         return false;
      }
      #endregion      

      #region Overrides of OpenType
      /// <summary>
      /// Returns true if provieded value is one of the following:
      /// <list type="bullet">
      /// <item>An enum of type same as the one used to create this <see cref="EnumerationType"/> instance.</item>
      /// <item>A string which is the name of one of this instance's legal values.</item>
      /// <item>Something convertible to Int32 (but not an enum!) and that Int32 is one of this instance's legal values.</item>
      /// </list>      
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public override bool IsValue(object value)
      {
         //if (value == null)
         //{
         //   return false;
         //}
         //if (value.GetType().AssemblyQualifiedName == _qualifiedTypeName)
         //{
         //   return true;
         //}
         //if (value.GetType().IsEnum)
         //{
         //   return false;
         //}
         //string stringValue = value as string;
         //if (stringValue != null)
         //{
         //   return HasValue(stringValue);
         //}
         //try
         //{
         //   int intValue = Convert.ToInt32(value);
         //   return HasValue(intValue);
         //}
         //catch (Exception)
         //{
         //   return false;
         //}
         IEnumerationData enumeration = value as IEnumerationData;
         return enumeration != null && enumeration.EnumerationType.Equals(this);
      }
      public override OpenTypeKind Kind
      {
         get { return OpenTypeKind.EnumerationType; }
      }      
      #endregion      

      #region Overridden
      public override bool Equals(object obj)
      {
         EnumerationType other = obj as EnumerationType;
         if (other != null && TypeName.Equals(other.TypeName) && other._legalValues.Count == _legalValues.Count)
         {
            for (int i = 0; i < _legalValues.Count; i++ )
            {
               KeyValuePair<string, int> thisPair = _legalValues[i];
               KeyValuePair<string, int> otherPair = other._legalValues[i];
               if ((thisPair.Key != otherPair.Key || thisPair.Value != otherPair.Value))
               {
                  return false;
               }
            }
            return true;
         }
         return false;               
      }      
      public override int GetHashCode()
      {
         int code = TypeName.GetHashCode();
         foreach (KeyValuePair<string, int> pair in _legalValues)
         {
            code ^= pair.Key.GetHashCode();
            code ^= pair.Value.GetHashCode();
         }
         return code;
      }
      #endregion

      private static string GetDescription(Type enumType)
      {
         if (!enumType.IsEnum)
         {
            throw new OpenDataException("EnumerationType requires to pass Enum type in constructor.");
         }
         if (enumType.IsDefined(typeof(DescriptionAttribute), false))
         {
            DescriptionAttribute attr =
               (DescriptionAttribute)enumType.GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
            return attr.Description;
         }
         return "Enumeration type";
      }
   }
}
