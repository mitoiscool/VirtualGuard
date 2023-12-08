using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.PE.DotNet.Metadata;
using VirtualGuard.RT.Chunk;
using VirtualGuard.RT.Descriptor;
using VirtualGuard.RT.Mutators;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT;

public class VirtualGuardRT
{
    public VirtualGuardRT(ModuleDefinition rtModule, bool debug = false)
    {
        RuntimeModule = rtModule;
        isDebug = debug;

        Elements = new VmElements()
        {
            VmEntry = rtModule.LookupMethod(RuntimeConfig.VmEntry),
            Constants = rtModule.LookupType(RuntimeConfig.Constants)
        };

        Descriptor = new VMDescriptor()
        {
            Data = new DataDescriptor(),
            Encryption = new EncryptionDescriptor(),
            OpCodes = new OpCodeDescriptor(new Random()),
            Watermark = new WatermarkDescriptor()
        };
        
    }
    
    public ModuleDefinition RuntimeModule;

    private Dictionary<VmChunk, MethodDefinition> _exportMap = new Dictionary<VmChunk, MethodDefinition>();

    public VmElements Elements;
    public bool isDebug = false;
    public VMDescriptor Descriptor;
    private List<VmChunk> _vmChunks = new List<VmChunk>();
    private List<IChunk> _additionalChunks = new List<IChunk>();
    
    private List<IChunk> _allChunks = new List<IChunk>();
    
    public int GetExportLocation(MethodDefinition def)
    {
        return ((VmChunk)_exportMap.Single(x => x.Value == def).Key).Offset;
    }
    
    public void AddExportChunk(VmChunk chunk, MethodDefinition parent)
    {
        AddChunk(chunk);
        _exportMap.Add(chunk, parent);
    }

    public void Inject(ModuleDefinition target)
    {
        var cloner = new MemberCloner(target);

        cloner.Include(RuntimeModule.TopLevelTypes);

        var res = cloner.Clone();

        foreach (var type in res.ClonedTopLevelTypes)
        {
            target.TopLevelTypes.Add(type);
        }
        
        // now update elements
        Elements.VmEntry = res.GetClonedMember(Elements.VmEntry);
        Elements.Constants = res.GetClonedMember(Elements.Constants);

        Elements.VmTypes = res.ClonedTopLevelTypes.ToArray();
    }
    
    public void AddChunk(VmChunk chunk) => _vmChunks.Add(chunk);

    public void AddChunk(IChunk chunk) => _additionalChunks.Add(chunk);

    public void WriteHeap(VirtualGuardContext ctx)
    {
        FinalizeChunks();
        
        MutateChunks();
        
        UpdateOffsets();

        var bytes = SerializeChunks();
        
        Print();
        
        // do encrypt stuff
        
        //ctx.Module.ToPEImage().DotNetDirectory.Metadata.Streams.Add(new CustomMetadataStream("#vg", bytes));
    }

    void MutateChunks()
    {
        foreach (var mutator in IRuntimeMutator.GetMutators())
        {
            mutator.Mutate(this);
        }
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


    void Print()
    {
        var sb = new StringBuilder();

        sb.AppendLine("VirtualGuard VM");

        sb.AppendLine();
        
        sb.AppendLine("------");
        sb.AppendLine("Exports");
        sb.AppendLine("------");

        foreach (var export in _exportMap)
        {
            sb.AppendLine($"Method: {export.Value.FullName} Offset: {export.Key.Offset}");
        }

        sb.AppendLine();
        
        sb.AppendLine("------");
        sb.AppendLine("Stats");
        sb.AppendLine("------");

        sb.AppendLine("Chunks: " + _allChunks.Count);
        sb.AppendLine("Exports: " + _exportMap.Count);
        
        sb.AppendLine("");
        
        sb.AppendLine("------");
        sb.AppendLine("Chunks:");
        sb.AppendLine("------");

        int i = 0;
        foreach (var chunk in _allChunks)
        {
            if(chunk is VmChunk vchunk)
                AppendVmChunk(sb, vchunk, i);

            sb.AppendLine();
            i++;
        }
        
        Console.WriteLine(sb.ToString());
    }

    void AppendVmChunk(StringBuilder sb, VmChunk chunk, int index)
    {
        sb.AppendLine("Chunk " + index + " - Offset: " + chunk.Offset + " Length: " + chunk.Length);

        foreach (var instr in chunk.Content)
        {
            var operandString = instr.Operand == null ? "" : instr.Operand.ToString();

            sb.AppendLine(instr.OpCode + " " + instr.Operand);
        }
        
        
        
    }
    
    
    
}