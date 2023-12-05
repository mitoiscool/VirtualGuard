using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Mul : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Push(ctx.Stack.Pop().ToNumeral().Mul(ctx.Stack.Pop().ToNumeral()));
        
        state = ExecutionState.Next;
    }

    public byte GetCode() => Constants.OP_MUL;
}