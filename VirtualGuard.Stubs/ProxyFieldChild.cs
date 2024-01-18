
namespace VirtualGuard.Stubs
{
    public class ProxyFieldChild : ProxyFieldBase
    {
        private object _obj;

        void InitializeValue()
        {
            
        }

        public override object GetValue()
        {
            return _obj;
        }

        public override void SetValue(object obj)
        {
            _obj = obj;
        }
    }
}