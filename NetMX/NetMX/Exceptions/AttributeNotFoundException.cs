#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public sealed class AttributeNotFoundException : OperationsException
	{
        private string _attributeName;
        /// <summary>
        /// Missing attribute name.
        /// </summary>
        public string AttributeName
        {
            get { return _attributeName; }
        }
        private string _objectName;
        /// <summary>
        /// ObjectName of MBean
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
        }
        private string _className;
        /// <summary>
        /// Class name of MBean
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeName">Name of missing attribute</param>
		/// <param name="objectName">ObjectName of MBean</param>
		/// <param name="className">Class name of MBean</param>
        public AttributeNotFoundException(string attributeName, string objectName, string className)
            :
            base(string.Format("Attribute \"{0}\" not found in MBean \"{1}\" of class \"{2}\"", attributeName, objectName, className))
        {
            _attributeName = attributeName;
            _objectName = objectName;
            _className = className;
        }
	}
}
