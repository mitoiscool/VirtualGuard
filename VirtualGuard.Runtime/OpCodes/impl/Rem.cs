using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Rem : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var res = ctx.Stack.Pop().ToNumeral().Rem(ctx.Stack.Pop().ToNumeral());
            ctx.Stack.Push(res);
            
            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}