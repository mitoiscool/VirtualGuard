using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Dup : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Stack.Push(ctx.Stack.Peek().Clone());

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}