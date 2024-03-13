using System.Reflection;

namespace VirtualGuard.Runtime.Variant.Reference.impl
{
    public class FieldReferenceVariant : BaseReferenceVariant
    {
        private FieldInfo _field;
        private object _inst;
        
        public FieldReferenceVariant(FieldInfo info, object inst)
        {
            _field = info;
            _inst = inst;
        }
        
        public override object GetObject()
        {
            return _field.GetValue(_inst);
        }

        public override void SetVariantValue(object obj)
        {
            _field.SetValue(_inst, obj);
        }

        public override BaseVariant Clone()
        {
            return new FieldReferenceVariant(_field, _inst);
        }
    }
}