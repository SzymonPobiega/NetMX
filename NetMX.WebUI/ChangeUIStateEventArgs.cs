using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// Provides information about what kind of change in UI behavior needs to be done.
   /// </summary>
   internal sealed class ChangeUIStateEventArgs : EventArgs
   {
      /// <summary>
      /// Gets object describing the control which initiated UI behavior change. 
      /// </summary>
      public object Selector { get; private set; }
      /// <summary>
      /// Gets new state of the user interface (enabled / disabled).
      /// </summary>
      public bool UIState { get; private set; }
      /// <summary>
      /// Creates new <see cref="ChangeUIStateEventArgs"/> instance.
      /// </summary>
      /// <param name="enabled">New state of the user interface (enabled / disabled).</param>
      /// <param name="selector">Object describing the control which initiated UI behavior change.</param>
      public ChangeUIStateEventArgs(bool enabled, object selector)
      {
         UIState = enabled;
         Selector = selector;
      }
   }
}
