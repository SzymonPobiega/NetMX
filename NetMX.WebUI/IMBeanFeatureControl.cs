using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.WebUI.WebControls
{   
   internal interface IMBeanFeatureControl
   {
      /// <summary>
      /// Sets the state of UI (enabled / disabled) of this control.
      /// </summary>
      /// <param name="enabled">Should the control be enabled for user interaction.</param>
      void SetUIState(bool enabled);
      /// <summary>
      /// Gets the base selector instance (not including current state of the control).
      /// </summary>
      /// <returns>Base selector instance.</returns>
      object Selector { get; }
      /// <summary>
      /// Sets the value of open type edited by user.
      /// </summary>
      /// <param name="currentSelector">Current selector (reflecting actual state of contol i.e. selected argument of operation control).</param>
      /// <param name="value">Edited value.</param>
      void SetOpenTypeValue(object currentSelector, object value);
   }      
}
