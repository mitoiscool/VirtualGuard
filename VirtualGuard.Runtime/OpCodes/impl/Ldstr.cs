using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldstr : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Stack.Push(ctx.Reader.ReadString(ctx.Reader.ReadInt()));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}