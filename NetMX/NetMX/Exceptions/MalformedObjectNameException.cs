#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[Serializable]
	public class MalformedObjectNameException : OperationsException
	{
        private string _objectName;
        /// <summary>
        /// Malformed ObjectName.
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }            
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objectName">Malformed ObjectName.</param>
        public MalformedObjectNameException(string objectName)
            : base(string.Format("\"{0}\" is not a valid ObjectName.", objectName))
        {
            _objectName = objectName; 
        }
	}
}
