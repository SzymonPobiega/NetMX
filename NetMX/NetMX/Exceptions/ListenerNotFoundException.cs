#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public sealed class ListenerNotFoundException : OperationsException
	{        
        private string _objectName;
        /// <summary>
        /// ObjectName of MBean
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
        }        
		/// <summary>
		/// Constructor.
		/// </summary>		
		/// <param name="objectName">ObjectName of MBean</param>		
        public ListenerNotFoundException(string objectName)
            :
            base(string.Format("Litener was not found in MBean \"{0}\"", objectName))
        {
            _objectName = objectName;
        }
	}
}
