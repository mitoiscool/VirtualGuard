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

    }
}