using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelem : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var arr = ctx.Stack.Pop().ToArray();
            
            ctx.Stack.Push(arr.LoadDelimeter(ctx.Stack.Pop()));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}