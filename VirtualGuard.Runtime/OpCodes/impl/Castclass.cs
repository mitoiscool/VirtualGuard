using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Castclass : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var type = ctx.ResolveType(ctx.Reader.ReadInt());
        
        ctx.Stack.Push(new ObjectVariant(Convert.ChangeType(ctx.Stack.Pop().GetObject(), type)));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}