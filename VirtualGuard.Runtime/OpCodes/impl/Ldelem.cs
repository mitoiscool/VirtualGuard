using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelem : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var delem = ctx.Stack.Pop();
            var arr = ctx.Stack.Pop().ToArray();
            
            ctx.Stack.Push(arr.LoadDelimeter(delem));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}