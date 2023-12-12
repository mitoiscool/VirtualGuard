using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Or : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        { 
            ctx.Stack.Push(ctx.Stack.Pop().ToNumeral().Or(ctx.Stack.Pop().ToNumeral()));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}