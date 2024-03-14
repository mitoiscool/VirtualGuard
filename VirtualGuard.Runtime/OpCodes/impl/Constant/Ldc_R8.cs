namespace VirtualGuard.Runtime.OpCodes.impl.Constant
{

    public class Ldc_R8 : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            throw new NotImplementedException();
        }

        public byte GetCode() => 0;
    }
}