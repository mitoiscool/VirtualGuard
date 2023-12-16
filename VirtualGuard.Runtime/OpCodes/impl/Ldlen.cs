using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Ldlen : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Push(ctx.Stack.Pop().ToArray().GetLength());
        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}