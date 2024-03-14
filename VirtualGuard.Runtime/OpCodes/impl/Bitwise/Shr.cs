namespace VirtualGuard.Runtime.OpCodes.impl.Bitwise;

public class Shr : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var factor = ctx.Stack.Pop();
        var num = ctx.Stack.Pop();
        
        ctx.Stack.Push(num.ToNumeral().Shr(factor.ToNumeral()));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}