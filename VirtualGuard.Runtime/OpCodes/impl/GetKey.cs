using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class GetKey : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(new UIntVariant(ctx.Reader.GetKey()));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}