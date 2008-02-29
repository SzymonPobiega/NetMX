#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public class MBeanRegistrationException : MBeanException
	{
        private string _phase;
        /// <summary>
        /// Registration phase.
        /// </summary>
        public string Phase
        {
            get { return _phase; }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="phase">Registration phase.</param>
        /// <param name="inner">Thrown exception.</param>
        public MBeanRegistrationException(string phase, Exception inner)
			: base(string.Format("Exception thrown in {0} phase", phase), inner)
		{
            _phase = phase;
		}
	}
}
