using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl.Pseudo;

public class RegionBuilder
{
    private VmCode _Code;
    private VmChunk _Chunk;
    
    public RegionBuilder(VmCode code)
    {
        _Code = code;
        _Chunk = new VmChunk(new List<VmInstruction>());
    }

    public RegionBuilder WithInstruction(VmCode code, object operand)
    {
        _Chunk.Content.Add(new VmInstruction(code, operand));
        return this;
    }
    
    public RegionBuilder WithInstruction(VmCode code)
    { // is this redundant?
        _Chunk.Content.Add(new VmInstruction(code));
        return this;
    }

    public VmCode GetCode() => _Code;
    public VmChunk GetChunk() => _Chunk;
    
    
}