#region Using
using System;
using System.Collections;
using NetMX.OpenMBean;
using System.Web.UI.WebControls;

#endregion

namespace NetMX.WebUI.WebControls
{
   internal static class ValueEditControlFactory
   {
      internal static IValueEditControl CreateValueEditControl(OpenType info)
      {
         throw new NotImplementedException();
         //return CreateValueEditControl(new NullOpenMBeanParameterInfo(info));
      }
      internal static IValueEditControl CreateValueEditControl(IOpenMBeanParameterInfo info)
      {         
         if (info == null)
         {
            return new TextValueEditControl();
         }
         string defaultValue = info.HasDefaultValue ? info.DefaultValue.ToString() : null;
         if (info.OpenType == SimpleType.Boolean)
         {
            return new ListValueEditControl(false.ToString(), new object[] {false, true});
         }
         if (info.OpenType == SimpleType.Character || info.OpenType == SimpleType.String || info.OpenType == SimpleType.ObjectName)
         {
            if (info.HasLegalValues)
            {
               return new ListValueEditControl(defaultValue, info.LegalValues);
            }
            return new TextValueEditControl(defaultValue);
         }
         if (info.OpenType == SimpleType.Byte)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, byte.MinValue, byte.MaxValue);
         }
         if (info.OpenType == SimpleType.Decimal)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Currency, decimal.MinValue, decimal.MaxValue);
         }
         if (info.OpenType == SimpleType.Double)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Double, null, null);
         }
         if (info.OpenType == SimpleType.Float)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Double, null, null);
         }
         if (info.OpenType == SimpleType.Integer)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, int.MinValue, int.MaxValue);
         }
         if (info.OpenType == SimpleType.Long)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, long.MinValue, long.MaxValue);
         }
         if (info.OpenType == SimpleType.Short)
         {
            return CreateForNumericType(info, defaultValue, ValidationDataType.Integer, short.MinValue, short.MaxValue);
         }
         throw new NotSupportedException();
      }
      private static IValueEditControl CreateForNumericType(IOpenMBeanParameterInfo info, string defaultValue, ValidationDataType dataType, object typeMinValue, object typeMaxValue)
      {
         if (info.HasLegalValues)
         {
            return new ListValueEditControl(defaultValue, info.LegalValues);
         }
         string minValue = info.HasMinValue ? info.MinValue.ToString() : (typeMinValue != null ? typeMinValue.ToString() : "");
         string maxValue = info.HasMaxValue ? info.MaxValue.ToString() : (typeMaxValue != null ? typeMaxValue.ToString() : "");
         return new TextValueEditControl(dataType, info.Name, defaultValue, minValue, maxValue);
      }      
   }
}