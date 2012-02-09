#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// This exception is raised when, in a role info, its minimum degree is greater than its maximum degree.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class InvalidRoleInfoException : RelationException
   {
      private string _roleName;
      /// <summary>
      /// Name of the role which cause the problem.
      /// </summary>
      public string RoleName
      {
        get { return _roleName; }
      }
      private int _minimumDegree;
      /// <summary>
      /// Gets minimum degree for corresponding role reference.
      /// </summary>
      public int MinimumDegree
      {
         get { return _minimumDegree; }
      }
      private int _maximumDegree;
      /// <summary>
      /// Gets maximum degree for corresponding role reference.
      /// </summary>
      public int MaximumDegree
      {
         get { return _maximumDegree; }
      }
      /// <summary>
      /// Creates new InvalidRoleInfoException object.
      /// </summary>      
      public InvalidRoleInfoException(string name, int minDegree, int maxDegree )
         : base()
      {
         _roleName = name;
         _minimumDegree = minDegree;
         _maximumDegree = maxDegree;
      }
      private InvalidRoleInfoException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         _roleName = info.GetString("roleName");
         _minimumDegree = info.GetInt32("minimumDegree");
         _maximumDegree = info.GetInt32("maximumDegree");
      }
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("roleName", _roleName);
         info.AddValue("minimumDegree", _minimumDegree);
         info.AddValue("maximumDegree", _maximumDegree);
      }
   }
}


