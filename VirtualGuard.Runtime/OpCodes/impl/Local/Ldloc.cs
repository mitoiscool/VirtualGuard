namespace VirtualGuard.Runtime.OpCodes.impl.Local
{

    public class Ldloc : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Push(ctx.Locals.GetLocal(ctx.Reader.ReadShort()));
            
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;

    }
}