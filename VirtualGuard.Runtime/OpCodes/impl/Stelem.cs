using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Stelem : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var arr = ctx.Stack.Pop().ToArray();

            arr.SetDelimeter(ctx.Stack.Pop(), ctx.Stack.Pop());
            state = ExecutionState.Next;
            
        }

        public byte GetCode() => 0;
    }
}