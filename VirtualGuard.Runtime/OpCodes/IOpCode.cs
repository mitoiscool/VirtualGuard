using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes;

public interface IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state);
    public byte GetCode();
}