#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX.OpenMBean;

#endregion

namespace NetMX.WebUI.WebControls
{
   public sealed class ViewEditOpenTypeEventArgs : EventArgs
   {
      private readonly bool _enableEdit;
      public bool EnableEdit
      {
         get { return _enableEdit; }
      }
      private readonly object _value;
      public object Value
      {
         get { return _value; }
      }
      private readonly OpenType _type;
      public OpenType Type
      {
         get { return _type; }
      }

      public ViewEditOpenTypeEventArgs(bool enableEdit, object value, OpenType type)
      {
         _enableEdit = enableEdit;
         _value = value;
         _type = type;
      }
   }
}
