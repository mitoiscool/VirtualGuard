using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class FlagJmp : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var loc = ctx.Stack.Pop();
            var jmpFlag = ctx.Stack.Pop();
            var flag = ctx.Stack.Pop();

            if(flag == jmpFlag)
                ctx.Reader.SetValue(loc.I4());

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}