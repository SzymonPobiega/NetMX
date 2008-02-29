#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public class OperationNotFoundException : OperationsException
	{
		private string _operationName;
        /// <summary>
        /// Missing operation name.
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
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
		/// <param name="operationName">Name of missing operation.</param>
		/// <param name="objectName">ObjectName of MBean.</param>
		/// <param name="className">Class name of MBean.</param>
        public OperationNotFoundException(string operationName, string objectName, string className)
            :
            base(string.Format("Operation \"{0}\" not found in MBean \"{1}\" of class \"{2}\"", operationName, objectName, className))
        {
            _operationName = operationName;
            _objectName = objectName;
            _className = className;
        }
	}
}
