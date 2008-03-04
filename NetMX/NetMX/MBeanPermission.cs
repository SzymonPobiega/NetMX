#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
#endregion

namespace NetMX
{
	public sealed class MBeanPermission : IPermission
	{
		#region MEMBERS
		private MBeanPermissionImpl _impl;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public MBeanPermission(string name, MBeanPermissionAction actions)
		{
			_impl = new MBeanPermissionImpl(name, actions);
		}
		public MBeanPermission(string className, string memberName, ObjectName objectName, MBeanPermissionAction actions)
		{
			_impl = new MBeanPermissionImpl(className, memberName, objectName, actions);
		}
		private MBeanPermission(MBeanPermissionImpl impl)
		{
			_impl = impl;
		}
		#endregion

		#region UTILITY
		#endregion

		#region OVERRIDDEN
		public override bool Equals(object obj)
		{
			MBeanPermission other = obj as MBeanPermission;
			if (other == null)
			{
				return false;
			}
			return this._impl.Equals(other._impl);
		}
		public override int GetHashCode()
		{
			return _impl.GetHashCode();
		}
		public override string ToString()
		{
			return _impl.ToString();
		}
		#endregion

		#region IPermission Members
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2103:ReviewImperativeSecurity")]
		public IPermission Copy()
		{
			return new MBeanPermission(this._impl.Copy());
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
		public void Demand()
		{
			_impl.VerifyAsNeeded();
			INetMXPrincipal principal = Thread.CurrentPrincipal as INetMXPrincipal;
			if (principal != null)
			{
				IEnumerable<MBeanPermission> heldPermissions = principal.Permissions; //Thread.GetData(Thread.GetNamedDataSlot("NetMX.MBeanPermission"));
				if (heldPermissions != null)
				{
					foreach (MBeanPermission held in heldPermissions)
					{
						held._impl.VerifyAsHeld();
						MBeanPermissionImpl thisImpl = this._impl;
						MBeanPermissionImpl heldImpl = held._impl;
						if ((thisImpl.ClassName == null || heldImpl.ClassName == "" || thisImpl.ClassName == heldImpl.ClassName) &&
							 (thisImpl.MemberName == null || heldImpl.MemberName == "" || thisImpl.MemberName == heldImpl.MemberName) &&
							 (thisImpl.ObjectName == null || heldImpl.ObjectName.Apply(thisImpl.ObjectName)) &&
							 (thisImpl.Actions & heldImpl.Actions) == thisImpl.Actions
							)
						{
							return;
						}
					}
					throw new SecurityException();
				}
			}
		}
		public IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			MBeanPermission other = target as MBeanPermission;
			if (other == null)
			{
				throw new ArgumentException("Incompatibile permission object.");
			}
			MBeanPermissionImpl result = _impl.Intersect(other._impl);
			return result != null ? new MBeanPermission(result) : null;
		}

		public bool IsSubsetOf(IPermission target)
		{
			MBeanPermission other = target as MBeanPermission;
			if (other == null)
			{
				throw new ArgumentException("Incompatibile permission object.");
			}
			return _impl.IsSubsetOf(other._impl);
		}

		public IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			MBeanPermission other = target as MBeanPermission;
			if (other == null)
			{
				throw new ArgumentException("Incompatibile permission object.");
			}
			MBeanPermissionImpl result = _impl.Union(other._impl);
			return result != null ? new MBeanPermission(result) : null;
		}
		#endregion

		#region ISecurityEncodable Members
		public void FromXml(SecurityElement e)
		{
			_impl.FromXml(e);
		}

		public SecurityElement ToXml()
		{
			return _impl.ToXml();
		}
		#endregion
	}
}
