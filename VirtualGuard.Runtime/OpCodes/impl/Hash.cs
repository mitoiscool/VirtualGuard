using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Hash : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(ctx.Stack.Pop().Hash());
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}