using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.RT.Dynamic;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor.Handler;

internal class VmHandler
{
    public VmHandler(VmCode code, TypeDefinition def, byte identifier)
    {
        OpCode = code;
        _internalDefinition = def;

        Identifier = identifier;
        FixupMutation = MutationExpression.Random(0);
        
        if(code == VmCode.__nop)
            return;

        var body = new CilMethodBody(_codeDefinition);
        _codeDefinition.CilMethodBody = body;
        
        body.Instructions.Add(CilOpCodes.Ldc_I4, identifier);
        body.Instructions.Add(CilOpCodes.Ret);

        if (OpCode == VmCode.Jmp)
            return;

        FixupMutation = MutationExpression.Random(1);
        
        // inline fixup mutation into the identifier body

        var fixupRefs = ExecuteDefinition.CilMethodBody.Instructions.Where(x =>
            x.Operand is IMethodDescriptor fd && fd.Name == "ReadFixupValue");

        if (!fixupRefs.Any())
            return;

        var fixupRef = fixupRefs.First();

        var mutationCil = this.FixupMutation.ToCIL();

        // get index of fixupRef
        var index = ExecuteDefinition.CilMethodBody.Instructions.IndexOf(fixupRef) + 1;

        // insert all into target
        ExecuteDefinition.CilMethodBody.Instructions.InsertRange(index, mutationCil);

        ExecuteDefinition.CilMethodBody.Instructions.CalculateOffsets();
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
    
    public byte EmulateFixupMutation(int fixup)
    {
        return (byte)FixupMutation.Solve(fixup);
    }

    public MutationExpression FixupMutation;
    public MethodDefinition ExecuteDefinition => HandlerDefinition.Methods.Single(x => x.Parameters.Count > 0);
    private MethodDefinition _codeDefinition => HandlerDefinition.Methods.Single(x => x.Parameters.Count == 0 && !x.IsConstructor);
    
    private byte? _internalIdentifier = null;
    private readonly TypeDefinition _internalDefinition = null;
}