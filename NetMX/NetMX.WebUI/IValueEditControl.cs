#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// Interface representing a control which purpose is to edit a siple value like text or number.
   /// </summary>
   public interface IValueEditControl
   {
      /// <summary>
      /// Textual form of value being edited.
      /// </summary>
      string Value { get; set; }
      /// <summary>
      /// Gets or sets the CSS class of the control. Automaticaly implemented because <see cref="IValueEditControl"/>
      /// implementations should be <see cref="WebControl"/> derived.
      /// </summary>
      string CssClass { get; set; }
      /// <summary>
      /// Gets or sets the ID of the control. Automaticaly implemented because <see cref="IValueEditControl"/>
      /// implementations should be <see cref="WebControl"/> derived.
      /// </summary>
      string ID { get; set; }
      /// <summary>
      /// Gets or sets if the control should use ViewState. Automaticaly implemented because <see cref="IValueEditControl"/>
      /// implementations should be <see cref="WebControl"/> derived.
      /// </summary>
      bool EnableViewState { get; set; }
      /// <summary>
      /// Gets or sets the visibility of the control. Automaticaly implemented because <see cref="IValueEditControl"/>
      /// implementations should be <see cref="WebControl"/> derived.
      /// </summary>
      bool Visible { get; set; }
   }
}