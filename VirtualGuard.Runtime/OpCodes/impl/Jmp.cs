using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Jmp : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Reader.SetValue(ctx.Stack.Pop().I4());
        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}