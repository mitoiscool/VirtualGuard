using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Pop : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Pop();
        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}