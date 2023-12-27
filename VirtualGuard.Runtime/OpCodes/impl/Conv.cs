using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Conv : IOpCode
{
    public void Execute(VMContext ctx)
    {
        
        // fml this code is absolute DOGSHIT

        var type = ctx.Stack.Pop().I4();
        var obj = ctx.Stack.Pop();

        if (type == Constants.CorlibID_I || type == Constants.CorlibID_U)
            throw new InvalidOperationException(Routines.EncryptDebugMessage("Ptr and UIntPtr not supported in conv."));
        
        if(type == Constants.CorlibID_I1 || type == Constants.CorlibID_I2 || type == Constants.CorlibID_I4)
            ctx.Stack.Push(new IntVariant(obj.I4()));
        
        if(type == Constants.CorlibID_U1 || type == Constants.CorlibID_U2 || type == Constants.CorlibID_U4)
            ctx.Stack.Push(new UIntVariant(obj.U4()));
        
        if(type == Constants.CorlibID_I8)
            ctx.Stack.Push(new LongVariant(obj.I8()));
        
        if(type == Constants.CorlibID_U8)
            ctx.Stack.Push(new ULongVariant(obj.U8()));

        ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}