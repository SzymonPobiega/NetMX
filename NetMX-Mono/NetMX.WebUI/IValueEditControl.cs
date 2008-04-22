#region Using
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace NetMX.WebUI.WebControls
{
   public interface IValueEditControl
   {
      string Value { get; set; }
      string CssClass { get; set; }
      string ID { get; set; }
      bool EnableViewState { get; set; }
      bool Visible { get; set; }
   }
}