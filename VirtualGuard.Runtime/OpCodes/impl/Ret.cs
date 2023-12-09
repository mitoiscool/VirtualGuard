using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ret : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            // we can make this into a pseudo-code by setting state var
            state = ExecutionState.Exit;
        }

        public byte GetCode() => 0;
    }
}