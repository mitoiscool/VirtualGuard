using System.Reflection.Emit;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl.Misc;

public class Sizeof : IOpCode
{
    private static DynamicMethod _cachedFactory;
    public void Execute(VMContext ctx)
    {
        if (_cachedFactory == null)
        {
            _cachedFactory = new DynamicMethod("virtualguard", typeof(int), new[] { typeof(Type) });
            var gen = _cachedFactory.GetILGenerator();
            
            gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Sizeof);
            gen.Emit(System.Reflection.Emit.OpCodes.Ret);
        }

        var type = ctx.ResolveType(ctx.Reader.ReadInt());

        var size = (int)_cachedFactory.Invoke(null, new[] { type });
        
        ctx.Stack.Push(new IntVariant(size));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}