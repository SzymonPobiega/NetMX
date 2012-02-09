#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Base class for all feature info classes.
   /// </summary>
	[Serializable]
	public abstract class MBeanFeatureInfo
	{
		private readonly string _name;
      /// <summary>
      /// Gets name of this feature.
      /// </summary>
		public string Name
		{
			get { return _name; }
		}
		private readonly string _description;
      /// <summary>
      /// Gets description of this feature.
      /// </summary>
		public string Description
		{
			get { return _description; }
		}

      private readonly Descriptor _descriptor;
      /// <summary>
      /// Gets descriptor of this feature.
      /// </summary>
      public Descriptor Descriptor
      {
         get { return _descriptor; }
      }

		protected MBeanFeatureInfo(string name, string description)
		{
			_name = name;
			_description = description;
         _descriptor = new Descriptor();
		}

      protected MBeanFeatureInfo(string name, string description, Descriptor descriptor)
      {
         _name = name;
         _description = description;
         _descriptor = descriptor; //TODO: copy values
      }
	}
}
