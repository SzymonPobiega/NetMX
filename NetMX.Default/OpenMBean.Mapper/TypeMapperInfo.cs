using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Server.OpenMBean.Mapper
{
   /// <summary>
   /// Provides information about a type mapper object.
   /// </summary>
   public sealed class TypeMapperInfo
   {
      private readonly int _priority;
      private readonly string _typeName;
      private readonly ObjectName _objectName;

      /// <summary>
      /// Creates new <see cref="TypeMapperInfo"/> object.
      /// </summary>
      /// <param name="priority">Priority of this mapper.</param>
      /// <param name="typeName">CLR type name if this is an internal mapper.</param>
      /// <param name="objectName"><see cref="ObjectName"/> if this is an external mapper.</param>
      public TypeMapperInfo(int priority, string typeName, ObjectName objectName)
      {
         _priority = priority;
         _objectName = objectName;
         _typeName = typeName;
      }

      /// <summary>
      /// Gets the priority of a mapper. Mappers are queried for handling types from lowest priorities to highest.
      /// </summary>
      public int Priority
      {
         get { return _priority; }
      }

      /// <summary>
      /// Gets the CLR type name of a mapper, if it was registered as an internal mapper (not an MBean).
      /// </summary>
      public string TypeName
      {
         get { return _typeName; }
      }

      /// <summary>
      /// Gets the <see cref="ObjectName"/> of a mapper, if it was registeref as an external mapper (MBean).
      /// </summary>
      public ObjectName ObjectName
      {
         get { return _objectName; }
      }
   }
}