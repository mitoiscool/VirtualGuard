using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Jz : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var loc = ctx.Stack.Pop();
            var flag = ctx.Stack.Pop().I2();
            var key = ctx.Reader.ReadByte();
            
            if (flag == 0)
            {
                ctx.Reader.SetKey(key.U1());
                ctx.Reader.SetValue(loc.I4());
            }

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}