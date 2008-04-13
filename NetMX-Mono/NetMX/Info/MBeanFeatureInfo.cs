#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#endregion

namespace NetMX
{
	[Serializable]
	public abstract class MBeanFeatureInfo
	{
		#region MEMBERS
		#endregion

		#region PROPERTIES
		private string _name;

		public string Name
		{
			get { return _name; }
		}
		private string _description;

		public string Description
		{
			get { return _description; }
		}
		#endregion

		#region CONSTRUCTOR        
		protected MBeanFeatureInfo(string name, string description)
		{
			_name = name;
			_description = description;
		}
		#endregion
	}
}
