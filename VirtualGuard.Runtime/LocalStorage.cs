using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime
{
    public class LocalStorage
    {
        private BaseVariant[] _internal;

        public LocalStorage()
        {
            _internal = new BaseVariant[10]; // if there's more than 10 locals fml
        }

        public BaseVariant GetLocal(BaseVariant index)
        {
            return _internal[index.I2()];
        }

        public void SetLocal(BaseVariant index, BaseVariant value)
        {
            _internal[index.I2()] = value;
        }
        
    }
}