using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace NetMX.Remote.Jsr262
{
   public sealed class ServerAddressExtension : IExtension<OperationContext>
   {
      private readonly Uri _address;

      public ServerAddressExtension(Uri address)
      {
         _address = address;
      }

      public Uri Address
      {
         get { return _address; }
      }

      public void Attach(OperationContext owner)
      {         
      }

      public void Detach(OperationContext owner)
      {         
      }
   }
}
