using System;

namespace VirtualGuard.Runtime.Variant.Object
{

    public class NullVariant : BaseVariant
    {
        public override object GetObject()
        {
            return null;
        }

        public override void SetValue(object obj)
        {
            throw new InvalidOperationException();
        }

        public override BaseVariant Clone()
        {
            return new NullVariant();
        }
    }
}