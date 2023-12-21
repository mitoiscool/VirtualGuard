using System.Text;
using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.PE.DotNet.Metadata;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using VirtualGuard.RT.Chunk;
using VirtualGuard.RT.Descriptor;
using VirtualGuard.RT.Mutators;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT;

public class VirtualGuardRT
{
    public VirtualGuardRT(ModuleDefinition rtModule, int debugKey, bool debug = false)
    {
        RuntimeModule = rtModule;
        isDebug = debug;

        Elements = new VmElements()
        {
            VmEntry = rtModule.LookupMethod(RuntimeConfig.VmEntry),
        };

        var rnd = new Random();

        Descriptor = new VMDescriptor()
        {
            Data = new DataDescriptor(rnd, debugKey)
            {
                StreamName = rnd.Next().ToString("x"),
                Watermark = rnd.Next().ToString("x")
            },
            OpCodes = new OpCodeDescriptor(rnd),
            ComparisonFlags = new ComparisonDescriptor(rnd),
            CorLibTypeDescriptor = new CorLibTypeDescriptor(rnd),
            ExceptionHandlers = new ExceptionHandlerDescriptor(rnd)
        };
        
    }
    
    public readonly ModuleDefinition RuntimeModule;

    private readonly Dictionary<VmChunk, MethodDefinition> _exportMap = new Dictionary<VmChunk, MethodDefinition>();
    private readonly Dictionary<VmChunk, MethodDefinition> _importMap = new Dictionary<VmChunk, MethodDefinition>();

    private Dictionary<VmChunk, byte> _chunkKeyMap = new Dictionary<VmChunk, byte>();
    
    public VmElements Elements;
    public readonly bool isDebug = false;
    public readonly VMDescriptor Descriptor;
    
    private readonly List<IChunk> _allChunks = new List<IChunk>();

    public HeaderChunk HeaderChunk;
    public List<IChunk> GetChunkList() => _allChunks;
    
    
    public int GetExportLocation(MethodDefinition def)
    {
        return ((VmChunk)_exportMap.Single(x => x.Value == def).Key).Offset;
    }
    
    public void AddExportChunk(VmChunk chunk, MethodDefinition parent)
    {
        AddChunk(chunk);
        _exportMap.Add(chunk, parent);
    }
    
    public void AddImportChunk(VmChunk chunk, MethodDefinition parent)
    {
        AddChunk(chunk);
        _importMap.Add(chunk, parent);
    }

    public void Inject(ModuleDefinition target)
    {
        var cloner = new MemberCloner(target);

        cloner.Include(RuntimeModule.TopLevelTypes.Where(x => !x.IsRuntimeSpecialName && !x.IsModuleType));

        var res = cloner.Clone();

        foreach (var type in res.ClonedTopLevelTypes)
        {
            target.TopLevelTypes.Add(type);
        }
        
        // now update elements
        Elements.VmEntry = res.GetClonedMember(Elements.VmEntry);
        //Elements.Constants = res.GetClonedMember(Elements.Constants); constants will be inlined, obsolete

        //Elements.VmTypes = res.ClonedTopLevelTypes.ToArray(); not needed, populated based off of injectedtypes
    }
    
    public void AddChunk(IChunk chunk) => _allChunks.Add(chunk);

    public void AddChunk(IChunk chunk, int index) => _allChunks.Insert(index, chunk);

    public int IndexOfChunk(IChunk chunk) => _allChunks.IndexOf(chunk);
    
    public VmChunk[] VmChunks => _allChunks.Where(x => x is VmChunk).Cast<VmChunk>().ToArray();

    public bool IsMethodVirtualized(MethodDefinition def, out VmChunk methodChunk)
    {
        methodChunk = null;
        
        bool isImport = _importMap.ContainsValue(def);
        bool isExport = _exportMap.ContainsValue(def);

        if (!isImport && !isExport)
            return false;

        if (isImport)
        {
            methodChunk = _importMap.Single(x => x.Value == def).Key;
        }
        else
        {
            methodChunk = _exportMap.Single(x => x.Value == def).Key;
        }

        return true;
    }
    
    public void BuildData(VirtualGuardContext ctx)
    {
        // add header chunk
        var headerChunk = new HeaderChunk(this);
        HeaderChunk = headerChunk;
        
        MutateChunks(ctx);
        
        UpdateOffsets();

        var bytes = SerializeChunks(ctx);
        
        Console.WriteLine(string.Join(',', bytes.Skip(headerChunk.Length)));
        
        Print();
        
        ctx.Module.Resources.Add(new ManifestResource(Descriptor.Data.StreamName, ManifestResourceAttributes.Private, new DataSegment(bytes)));
        //ctx.Module.ToPEImage().DotNetDirectory.Metadata.Streams.Add(new CustomMetadataStream("#vg", bytes));
    }

    void MutateChunks(VirtualGuardContext ctx)
    {
        foreach (var mutator in IRuntimeMutator.GetMutators())
        {
            mutator.Mutate(this, ctx);
        }
    }

    byte[] SerializeChunks(VirtualGuardContext ctx)
    {
        var ms = new MemoryStream();
        var binaryWriter = new BinaryWriter(ms);
        
        // add all chunks together

        HeaderChunk.WriteBytes(binaryWriter, this);

        foreach (var chunk in _allChunks)
        {
            chunk.WriteBytes(binaryWriter, this);
        }

        return ms.ToArray();
    }
    
    public void UpdateOffsets() // keep in mind this uses all chunks, so do this when the bytes are requested
    {
        int offset = 0;

        offset += HeaderChunk.Length;
        
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
            sb.AppendLine($"Method: {export.Value.FullName} Offset: {export.Key.Offset} Entry: {Descriptor.Data.GetStartKey(export.Key)}");
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
        int offset = HeaderChunk.Length;
        
        foreach (var chunk in _allChunks)
        {
            if(chunk is VmChunk vchunk)
                AppendVmChunk(sb, vchunk, i, ref offset);
            else
            {
                sb.AppendLine(chunk.GetType().Name + " " + i + " - length: " + chunk.Length);
                offset += chunk.Length;
            }

            sb.AppendLine();
            i++;
        }

        sb.AppendLine("flags:");
        sb.AppendLine("gt: " + Descriptor.ComparisonFlags.GtFlag);
        sb.AppendLine("lt: " + Descriptor.ComparisonFlags.LtFlag);
        sb.AppendLine("eq: " + Descriptor.ComparisonFlags.EqFlag);
        
        Console.WriteLine(sb.ToString());
    }

    void AppendVmChunk(StringBuilder sb, VmChunk chunk, int index, ref int offset)
    {
        sb.AppendLine("Chunk " + index + " - Offset: " + chunk.Offset + " Length: " + chunk.Length + " StartKey: " + + Descriptor.Data.GetStartKey(chunk));

        foreach (var instr in chunk.Content)
        {
            var operandString = instr.Operand == null ? "" : instr.Operand.ToString();
            
            sb.AppendLine(offset + " - " + instr.OpCode + " " + operandString);
            
            offset += instr.GetSize();
        }
        
        
        
    }
    
    
    
}