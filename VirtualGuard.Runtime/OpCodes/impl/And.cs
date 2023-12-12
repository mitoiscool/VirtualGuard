using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class And : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Stack.Push(ctx.Stack.Pop().ToNumeral().And(ctx.Stack.Pop().ToNumeral()));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}