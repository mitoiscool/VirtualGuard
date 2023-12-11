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
        
        public override void SetValue(object obj)
        {
            _variant.SetValue(obj);
        }

        public override BaseVariant Clone()
        {
            return new LocalReferenceVariant(_variant);
        }
        
        
        
    }
}