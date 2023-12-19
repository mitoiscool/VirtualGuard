using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.Handlers;

public class Marker : CilInstruction
{

    public Marker(CilOpCode opCode, MarkerType t) : base(opCode)
    {
        Type = t;
    }

    public Marker(int offset, CilOpCode opCode) : base(offset, opCode)
    {
    }

    public Marker(CilOpCode opCode, object? operand) : base(opCode, operand)
    {
    }

    public Marker(int offset, CilOpCode opCode, object? operand) : base(offset, opCode, operand)
    {
    }

    public MarkerType Type;

    public override string ToString()
    {
        switch (Type)
        {
            case MarkerType.TryStart:
                return "<Enter Try>";
            
            case MarkerType.TryEnd:
                return "<Leave Try>";
            
            case MarkerType.HandlerStart:
                return "<Enter Handler>";
            
            case MarkerType.HandlerEnd:
                return "<Leave Hander>";
            
            default:
                return "<Marker>";
        }
    }
}