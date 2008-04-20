#region Using
using System;
using NetMX.OpenMBean;
using System.Web.UI.WebControls;

#endregion

namespace NetMX.WebUI.WebControls
{
   public static class ValueEditControlFactory
   {
      public static IValueEditControl CreateValueEditControl(IOpenMBeanParameterInfo info)
      {         
         if (info == null)
         {
            return new TextValueEditControl();
         }
         else
         {
            string defaultValue = info.HasDefaultValue ? info.DefaultValue.ToString() : null;
            if (info.OpenType == SimpleType.Boolean)
            {
               return new ListValueEditControl(false.ToString(), new object[] {false, true});
            }
            else if (info.OpenType == SimpleType.Character || info.OpenType == SimpleType.String || info.OpenType == SimpleType.ObjectName)
            {
               if (info.HasLegalValues)
               {
                  return new ListValueEditControl(defaultValue, info.LegalValues);
               }
               else
               {
                  return new TextValueEditControl(defaultValue);
               }
            }            
            else if (info.OpenType == SimpleType.Byte)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, byte.MinValue, byte.MaxValue);
            }
            else if (info.OpenType == SimpleType.Decimal)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Currency, decimal.MinValue, decimal.MaxValue);
            }
            else if (info.OpenType == SimpleType.Double)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Double, double.MinValue, double.MaxValue);
            }
            else if (info.OpenType == SimpleType.Float)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Double, float.MinValue, float.MaxValue);
            }
            else if (info.OpenType == SimpleType.Integer)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, int.MinValue, int.MaxValue);
            }
            else if (info.OpenType == SimpleType.Long)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, long.MinValue, long.MaxValue);
            }
            else if (info.OpenType == SimpleType.Short)
            {
               return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, short.MinValue, short.MaxValue);
            }
            else
            {
               throw new NotSupportedException();
            }
         }
      }
      private static IValueEditControl CreateForNumericType(IOpenMBeanParameterInfo info, string defaultValue, ValidationDataType dataType, object typeMinValue, object typeMaxValue)
      {
         if (info.HasLegalValues)
         {
            return new ListValueEditControl(defaultValue, info.LegalValues);
         }
         else
         {
            string minValue = info.HasMinValue ? info.MinValue.ToString() : typeMinValue.ToString();
            string maxValue = info.HasMaxValue ? info.MaxValue.ToString() : typeMaxValue.ToString();
            return new TextValueEditControl(dataType, info.Name, defaultValue, minValue, maxValue); 
         }
      }
   }
}