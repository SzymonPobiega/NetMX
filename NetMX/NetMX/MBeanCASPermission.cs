using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace NetMX
{
    [Serializable]
    [ComVisible(true)]
    public sealed class MBeanCASPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        #region MEMBERS
        private MBeanPermissionImpl _impl;
        #endregion

        #region CONSTRUCTOR        
        public MBeanCASPermission(PermissionState state)
        {            
            _impl = new MBeanPermissionImpl(null, null, null, MBeanPermissionAction.All);
        }
        public MBeanCASPermission(string name, MBeanPermissionAction actions)
		{
            _impl = new MBeanPermissionImpl(name, actions);
		}
        public MBeanCASPermission(string className, string memberName, ObjectName objectName, MBeanPermissionAction actions)
		{
            _impl = new MBeanPermissionImpl(className, memberName, objectName, actions);
		}
        private MBeanCASPermission(MBeanPermissionImpl impl)
        {
            _impl = impl;
        }
		#endregion

        #region OVERRIDDEN
        public override bool Equals(object obj)
        {
            MBeanCASPermission other = obj as MBeanCASPermission;
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
        public override IPermission Copy()
        {
            return new MBeanCASPermission(_impl.Copy());
        }        

        public override IPermission Intersect(IPermission target)
        {
            if (target == null)
            {
                return null;
            }
            MBeanCASPermission other = target as MBeanCASPermission;
            if (other == null)
            {
                throw new ArgumentException("Incompatibile permission object.");
            }
            MBeanPermissionImpl result = _impl.Intersect(other._impl);
            return result != null ? new MBeanCASPermission(result) : null;
        }

        public override IPermission Union(IPermission target)
        {
            if (target == null)
            {
                return this.Copy();
            }
            MBeanCASPermission other = target as MBeanCASPermission;
            if (other == null)
            {
                throw new ArgumentException("Incompatibile permission object.");
            }
            MBeanPermissionImpl result = _impl.Union(other._impl);
            return result != null ? new MBeanCASPermission(result) : null;
        }

        public override bool IsSubsetOf(IPermission target)
        {
            if (target == null)
            {
                return false;
            }
            MBeanCASPermission other = target as MBeanCASPermission;
			if (other == null)
			{
				throw new ArgumentException("Incompatibile permission object.");
			}
            return _impl.IsSubsetOf(other._impl);
        }

        public override SecurityElement ToXml()
        {
            return _impl.ToXml();
        }
        public override void FromXml(SecurityElement elem)
        {
            _impl.FromXml(elem);
        }
        #endregion

        #region IUnrestrictedPermission Members
        public bool IsUnrestricted()
        {
            return false;
        }
        #endregion
    }
}
