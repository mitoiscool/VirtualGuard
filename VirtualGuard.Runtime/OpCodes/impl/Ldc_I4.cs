using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Ldc_I4 : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Push(ctx.Reader.ReadInt());

        state = ExecutionState.Next;
    }

    public byte GetCode() => Constants.OP_LDC_I4;
}