using System.Reflection.Emit;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Sizeof : IOpCode
{
    private static DynamicMethod _cachedFactory;
    public void Execute(VMContext ctx)
    {
        if (_cachedFactory == null)
        {
            
        }
        
        

        _cachedFactory.Invoke(null, new[] { });
    }

    public byte GetCode() => 0;
}