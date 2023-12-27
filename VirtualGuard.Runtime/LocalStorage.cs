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

        public BaseVariant GetLocal(short index)
        {
            if (index > _internal.Length)
            {
                var v = _internal;
                _internal = new BaseVariant[v.Length * 2];
                Array.Copy(v, _internal, v.Length);
            }
                
            return _internal[index];
        }

        public void SetLocal(short index, BaseVariant value)
        {
            if (index > _internal.Length)
            {
                var v = _internal;
                _internal = new BaseVariant[v.Length * 2];
                Array.Copy(v, _internal, v.Length);
            }
            
            _internal[index] = value;
        }
        
    }
}