using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Sub : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var i2 = ctx.Stack.Pop().ToNumeral();
            var i1 = ctx.Stack.Pop().ToNumeral();

            ctx.Stack.Push(i1.Sub(i2));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}

