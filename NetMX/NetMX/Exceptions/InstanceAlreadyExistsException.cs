#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[Serializable]
	public class InstanceAlreadyExistsException : OperationsException
	{
        private string _objectName;
        /// <summary>
        /// ObjectName
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objectName">ObjectName</param>
        public InstanceAlreadyExistsException(string objectName)
            : base(string.Format("ObjectName \"{0}\" is already used by another MBean.", objectName))
        {
            _objectName = objectName;
        }
	}
}
