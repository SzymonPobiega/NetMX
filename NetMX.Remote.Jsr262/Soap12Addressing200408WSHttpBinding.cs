using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace NetMX.Remote.Jsr262
{
   public class Soap12Addressing200408WSHttpBinding : WSHttpBinding
   {
      public Soap12Addressing200408WSHttpBinding(SecurityMode securityMode)
         : base(securityMode)
      { }

      public Soap12Addressing200408WSHttpBinding(string configurationName)         
      {
         Soap12Addressing200408WSHttpBindingCollectionElement.GetBindingCollectionElement()
            .Bindings[configurationName]
            .ApplyConfiguration(this);
      }      
      
      public override BindingElementCollection CreateBindingElements()
      {
         BindingElementCollection elements = base.CreateBindingElements();
         TextMessageEncodingBindingElement txtenc = elements.Find<TextMessageEncodingBindingElement>();
         txtenc.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
         return elements;
      }
   }
}