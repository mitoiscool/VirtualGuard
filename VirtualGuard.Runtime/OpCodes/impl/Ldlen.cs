using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Ldlen : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(ctx.Stack.Pop().ToArray().GetLength());
        
        ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}