namespace VirtualGuard.Runtime.Variant.Object
{

    public class ObjectVariant : BaseVariant
    {
        public ObjectVariant(object obj)
        {
            _obj = obj;
        }

        private object _obj;

        public override object GetObject()
        {
            return _obj;
        }

        public override void SetValue(object obj)
        {
            _obj = obj;
        }

        public override BaseVariant Clone()
        {
            return new ObjectVariant(_obj);
        }
    }
}