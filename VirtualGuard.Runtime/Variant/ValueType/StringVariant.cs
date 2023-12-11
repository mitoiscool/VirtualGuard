namespace VirtualGuard.Runtime.Variant.ValueType
{

    public class StringVariant : BaseVariant
    {
        private string _str;

        public StringVariant(string str)
        {
            _str = str;
        }

        public override object GetObject()
        {
            return _str;
        }

        public override void SetValue(object obj)
        {
            _str = (string)obj;
        }

        public override BaseVariant Clone()
        {
            return new StringVariant(_str);
        }
    }
}