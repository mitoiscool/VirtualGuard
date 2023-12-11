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
            return new Local
        }

        public void SetLocal(BaseVariant index, BaseVariant value)
        {
            
        }
        
        
    }
}