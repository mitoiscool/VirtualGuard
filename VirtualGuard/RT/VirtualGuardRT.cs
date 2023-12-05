using AsmResolver.DotNet;
using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT;

public class VirtualGuardRT
{
    public VirtualGuardRT(ModuleDefinition rtModule)
    {
        RuntimeModule = rtModule;
    }
    
    public ModuleDefinition RuntimeModule;

    private Dictionary<VmChunk, MethodDefinition> _exportMap = new Dictionary<VmChunk, MethodDefinition>();
    
    private List<VmChunk> _vmChunks = new List<VmChunk>();
    private List<IChunk> _additionalChunks = new List<IChunk>();
    
    private List<IChunk> _allChunks = new List<IChunk>();

    public void AddExportChunk(VmChunk chunk, MethodDefinition parent)
    {
        AddChunk(chunk);
        _exportMap.Add(chunk, parent);
    }
    public void AddChunk(VmChunk chunk) => _vmChunks.Add(chunk);

    public void AddChunk(IChunk chunk) => _additionalChunks.Add(chunk);

    public void RequestHeap(VirtualGuardContext ctx)
    {
        FinalizeChunks();
        
        MutateChunks();
        
        UpdateOffsets();

        var bytes = SerializeChunks();
        
        
    }

    void MutateChunks()
    {
        
    }

    byte[] SerializeChunks()
    {
        var ms = new MemoryStream();
        var binaryWriter = new BinaryWriter(ms);
        
        // add all chunks together

        foreach (var chunk in _allChunks)
        {
            chunk.WriteBytes(binaryWriter);
        }

        return ms.ToArray();
    }
    
    void FinalizeChunks()
    {
        _allChunks.Clear();
        _allChunks.AddRange(_additionalChunks);
        _allChunks.AddRange(_vmChunks);
    }
    void UpdateOffsets() // keep in mind this uses all chunks, so do this when the bytes are requested
    {
        int offset = 0;
        
        foreach (var chunk in _allChunks)
        {
            chunk.OnOffsetComputed(offset);
            offset += chunk.Length;
        }
        
    }

}