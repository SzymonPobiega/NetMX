#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

#endregion

namespace NetMX.WebUI.WebControls
{
   public class ListValueEditControl : DropDownList, IValueEditControl
   {      
      public ListValueEditControl(string defaultValue, IEnumerable values)
      {
         foreach (object val in values)
         {
            string name = val.ToString();
            Items.Add(new ListItem(name, name));
         }
         if (defaultValue != null)
         {
            SelectedValue = defaultValue;
         }
      }

      #region IValueEditControl Members
      public string Value
      {
         get
         {
            return SelectedValue;
         }
         set
         {
            SelectedValue = value;
         }
      }
      #endregion
   }
}