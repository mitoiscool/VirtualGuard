using System.Reflection;

namespace VirtualGuard.Runtime.Variant.Member.impl
{

    public class FieldVariant : BaseMemberVariant
    {
        private FieldInfo _info;

        public FieldVariant(FieldInfo info)
        {
            _info = info;
        }

        public override object GetObject()
        {
            return _info;
        }

        public override void SetValue(BaseVariant inst, BaseVariant value)
        {
            _info.SetValue(inst.GetObject(), value.GetObject());
        }

        public override object GetValue(BaseVariant inst)
        {
            return _info.GetValue(inst.GetObject());
        }
    }
}