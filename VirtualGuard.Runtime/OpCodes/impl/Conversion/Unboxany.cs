namespace VirtualGuard.Runtime.OpCodes.impl.Conversion;

public class Unboxany : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var type = ctx.ResolveType(ctx.Reader.ReadInt());
        
        ctx.Stack.Push(ctx.Stack.Pop().Unbox().Cast(type));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}