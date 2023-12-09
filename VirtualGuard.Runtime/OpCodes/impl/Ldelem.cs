using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelem : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            throw new System.NotImplementedException();
        }

        public byte GetCode() => 0;
    }
}