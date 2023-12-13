using System.Reflection;

namespace VirtualGuard.Runtime.Variant.Reference.impl
{
    public class FieldReferenceVariant : BaseReferenceVariant
    {
        public FieldReferenceVariant(FieldInfo info, object inst)
        {
            
        }
        
        public override object GetObject()
        {
            throw new System.NotImplementedException();
        }

        public override void SetVariantValue(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override BaseVariant Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}