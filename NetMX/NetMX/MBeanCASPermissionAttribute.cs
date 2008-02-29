#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Security;
using System.Runtime.InteropServices;
#endregion

namespace NetMX
{
    [Serializable]
    [ComVisible(true)]
    public sealed class MBeanCASPermissionAttribute : CodeAccessSecurityAttribute
    {
        #region PROPERTIES
        private string _className;
        /// <summary>
        /// Class name 
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        private string _memberName;
        /// <summary>
        /// Member name
        /// </summary>
        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }
        private string _objectName;
        /// <summary>
        /// Object name
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }
        private MBeanPermissionAction _access = MBeanPermissionAction.All;
        /// <summary>
        /// Access
        /// </summary>
        public MBeanPermissionAction Access
        {
            get { return _access; }
            set { _access = value; }
        }        
        #endregion

        #region CONSTRUCTROS
        public MBeanCASPermissionAttribute(SecurityAction securityAction)
            : base(securityAction)
        {            
        }
        #endregion

        #region OVERRIDDEN
        public override IPermission CreatePermission()
        {
            return new MBeanCASPermission(_className, _memberName, _objectName != null ? new ObjectName(_objectName) : null, _access);
        }
        #endregion
    }
}
