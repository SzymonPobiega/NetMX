#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[Serializable]
	public class InstanceNotFoundException : OperationsException
	{
        string _objectName;
        /// <summary>
        /// ObjectName of missing MBean.
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
        }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="objectName">ObjectName of missing MBean.</param>
        public InstanceNotFoundException(string objectName)
            : base(string.Format("MBean of name \"{0}\" does not exist in this MBeanServer.", objectName))
        {
            _objectName = objectName;
        }
	}
}
