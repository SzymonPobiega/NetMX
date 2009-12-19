﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.Management
{
   public sealed class FragmentTransferHeader : MessageHeader
   {
      public const string ElementName = "FragmentTransfer";      
      private readonly string _expression;

      public FragmentTransferHeader(string expression)
      {
         _expression = expression;
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {         
         writer.WriteValue(_expression);         
      }
      

      public static FragmentTransferHeader ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, Const.Namespace);         
         StringBuilder fragment = new StringBuilder();
         while (reader.NodeType == XmlNodeType.Text)
         {
            fragment.Append(reader.Value);
            reader.Read();
         }
         FragmentTransferHeader result = new FragmentTransferHeader(fragment.ToString());
         reader.ReadEndElement();         
         return result;
      }

      public static FragmentTransferHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static FragmentTransferHeader ReadFrom(MessageHeaders messageHeaders)
      {
         FragmentTransferHeader result;
         int index = messageHeaders.FindHeader(ElementName, Const.Namespace);
         if (index < 0)
         {
            return null;
         }
         using (XmlDictionaryReader readerAtHeader = messageHeaders.GetReaderAtHeader(index))
         {
            result = ReadFrom(readerAtHeader);
         }
         MessageHeaderInfo headerInfo = messageHeaders[index];
         if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
         {
            messageHeaders.UnderstoodHeaders.Add(headerInfo);
         }
         return result;
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Const.Namespace; }
      }

      public string Expression
      {
         get { return _expression; }
      }
   }
}