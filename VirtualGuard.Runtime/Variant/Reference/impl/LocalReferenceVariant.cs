using System;

namespace VirtualGuard.Runtime.Variant.Reference.impl
{
    public class LocalReferenceVariant : BaseReferenceVariant
    {
        private BaseVariant _variant;
        
        public LocalReferenceVariant(BaseVariant var)
        {
            _variant = var;
        }
        
        public override object GetObject()
        {
            return _variant.GetObject();
        }
        
        public override void SetVariantValue(object obj)
        {
            _variant.SetVariantValue(obj);
        }

        public override BaseVariant Clone()
        {
            return new LocalReferenceVariant(_variant);
        }
        
        
        
    }
}