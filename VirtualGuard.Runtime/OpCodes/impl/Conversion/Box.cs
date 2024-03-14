namespace VirtualGuard.Runtime.OpCodes.impl.Conversion;

public class Box : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var type = ctx.ResolveType(ctx.Reader.ReadInt());

        ctx.Stack.Push(ctx.Stack.Pop().Cast(type).Box());
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}