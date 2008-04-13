using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace NetMX.Default
{
    internal class MBeanRegistrationHelper :IMBeanRegistration
    {
        private IMBeanRegistration _inner;

        public MBeanRegistrationHelper(IMBeanRegistration inner)
        {
            _inner = inner;
        }

        #region IMBeanRegistration Members
        public void PostDeregister()
        {
            if (_inner != null)
            {
                try
                {
                    _inner.PostDeregister();
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        throw;
                    }
                    throw new MBeanRegistrationException("PostDeregister.", ex);
                }
            }
        }
        public void PostRegister(bool registrationDone)
        {
            if (_inner != null)
            {
                try
                {
                    _inner.PostRegister(registrationDone);
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        throw;
                    }
                    throw new MBeanRegistrationException("PostRegister.", ex);
                }
            }
        }
        public void PreDeregister()
        {
            if (_inner != null)
            {
                try
                {
                    _inner.PreDeregister();
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        throw;
                    }
                    throw new MBeanRegistrationException("PreDeregister.", ex);
                }
            }
        }
        public ObjectName PreRegister(IMBeanServer server, ObjectName name)
        {
            if (_inner != null)
            {
                try
                {
                    return _inner.PreRegister(server, name);
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        throw;
                    }
                    throw new MBeanRegistrationException("PreRegister.", ex);
                }
            }
            else
            {
                return name;
            }
        }
        #endregion
    }
}
