using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Hash : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Push(ctx.Stack.Pop().Hash());
        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}