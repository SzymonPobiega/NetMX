using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace NetMX.Remote.ServiceModel
{
   public class NetDataContractFormatAttribute : Attribute, IOperationBehavior
   {
      #region IOperationBehavior Members
      public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
      {
      }

      public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
      {
         ReplaceDataContractSerializerOperationBehavior(description);
      }

      public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
      {
         ReplaceDataContractSerializerOperationBehavior(description);
      }

      public void Validate(OperationDescription description)
      {
      }
      #endregion

      private static void ReplaceDataContractSerializerOperationBehavior(
         OperationDescription description)
      {
         var dcsOperationBehavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
         if (dcsOperationBehavior != null)
         {
            description.Behaviors.Remove(dcsOperationBehavior);
            description.Behaviors.Add(new NetDataContractSerializerOperationBehavior(description));
         }
      }

      #region Nested type: NetDataContractSerializerOperationBehavior

      public class NetDataContractSerializerOperationBehavior :
         DataContractSerializerOperationBehavior
      {
         public NetDataContractSerializerOperationBehavior(OperationDescription operationDescription) :
               base(operationDescription)
         {
         }

         public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
         {
            return new NetDataContractSerializer();
         }

         public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
         {
            return new NetDataContractSerializer();
         }
      }
      #endregion      
   }
}