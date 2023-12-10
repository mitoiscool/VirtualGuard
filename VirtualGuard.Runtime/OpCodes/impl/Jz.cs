using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Flags;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Jz : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var flag = ctx.Stack.Pop().I2();
            var loc = ctx.Stack.Pop();
            if (flag == 0)
            {
                ctx.Reader.SetValue(loc.I4());
            }

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}