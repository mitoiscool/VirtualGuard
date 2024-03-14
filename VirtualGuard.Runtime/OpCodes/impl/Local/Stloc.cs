namespace VirtualGuard.Runtime.OpCodes.impl.Local
{

    public class Stloc : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Locals.SetLocal(ctx.Reader.ReadShort(), ctx.Stack.Pop());

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}