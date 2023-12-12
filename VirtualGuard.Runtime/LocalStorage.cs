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
            if (index.I2() > _internal.Length)
            {
                var v = _internal;
                _internal = new BaseVariant[v.Length * 2];
                Array.Copy(v, _internal, v.Length);
            }
                
            return _internal[index.I2()];
        }

        public void SetLocal(BaseVariant index, BaseVariant value)
        {
            if (index.I2() > _internal.Length)
            {
                var v = _internal;
                _internal = new BaseVariant[v.Length * 2];
                Array.Copy(v, _internal, v.Length);
            }
            
            _internal[index.I2()] = value;
        }
        
    }
}