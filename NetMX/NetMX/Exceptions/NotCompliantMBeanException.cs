#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public sealed class NotCompliantMBeanException : OperationsException
	{        
        private string _className;
        /// <summary>
        /// Class name of not compliant MBean.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objectName">ObjectName of not compliant MBean.</param>
		public NotCompliantMBeanException(string className) 
            : base(string.Format("MBean class \"{0}\" is not compliant with NetMX specification.", className)) 
        {
            _className = className;
        }
	}
}
