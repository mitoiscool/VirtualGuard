using System;
using System.Text;

namespace VirtualGuard.Stubs
{
    public abstract class ProxyFieldBase
    {
        public virtual object GetValue()
        {
            return "via https://virtualguard.io/";
        }

        public virtual void SetValue(object obj)
        {
            Environment.Exit(-186414);
        }
    }
}