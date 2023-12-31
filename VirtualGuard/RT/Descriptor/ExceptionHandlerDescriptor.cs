using AsmResolver.DotNet.Code.Cil;

namespace VirtualGuard.RT.Descriptor;

internal class ExceptionHandlerDescriptor
{
    public ExceptionHandlerDescriptor(Random rnd)
    {
        CatchFL = (byte)rnd.Next(byte.MaxValue);
        FinallyFL = (byte)rnd.Next(byte.MaxValue);
        FilterFL = (byte)rnd.Next(byte.MaxValue);
        FaultFL = (byte)rnd.Next(byte.MaxValue);
    }

    public byte CatchFL;
    public byte FinallyFL;
    public byte FilterFL;
    public byte FaultFL;

    public byte GetFlag(CilExceptionHandlerType type)
    {
        switch (type)
        {
            case CilExceptionHandlerType.Exception:
                return CatchFL;
            
            case CilExceptionHandlerType.Fault:
                return FaultFL;
            
            case CilExceptionHandlerType.Filter:
                return FilterFL;
            
            case CilExceptionHandlerType.Finally:
                return FinallyFL;
        }

        throw new InvalidOperationException(type.ToString());
    }
}