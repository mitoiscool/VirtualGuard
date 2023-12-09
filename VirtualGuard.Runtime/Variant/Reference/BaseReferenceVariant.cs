using System;

namespace VirtualGuard.Runtime.Variant.Reference
{

    public abstract class BaseReferenceVariant : BaseVariant
    {
        public override bool IsReference()
        {
            return true;
        }

        public abstract IntPtr GetPtr();
    }
}