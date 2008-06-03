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

      private readonly string _description;
      public string Description
      {
         get { return _description; }
      }

      public ViewEditOpenTypeEventArgs(bool enableEdit, object value, OpenType type, string description)
      {
         _enableEdit = enableEdit;
         _value = value;
         _type = type;
         _description = description;
      }
   }
}
