using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor.Handler;

public class VmHandler
{
    public VmHandler(VmCode code, TypeDefinition def, byte identifier)
    {
        OpCode = code;
        _internalDefinition = def;

        Identifier = identifier;

        var body = new CilMethodBody(_codeDefinition);
        _codeDefinition.CilMethodBody = body;
        
        body.Instructions.Add(CilOpCodes.Ldc_I4, identifier);
        body.Instructions.Add(CilOpCodes.Ret);
    }
    
    public readonly VmCode OpCode;
    public byte Identifier;

    public TypeDefinition HandlerDefinition
    {
        get
        {
            if (_internalDefinition == null)
                throw new Exception("Handler Representation " + OpCode + " never initialized!");

            return _internalDefinition;
        }
    }

    public MethodDefinition ExecuteDefinition => HandlerDefinition.Methods.Single(x => x.Parameters.Count > 0);
    
    private MethodDefinition _codeDefinition => HandlerDefinition.Methods.Single(x => x.Parameters.Count == 0 && !x.IsConstructor);
    
    private byte? _internalIdentifier = null;
    private readonly TypeDefinition _internalDefinition = null;
}