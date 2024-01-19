using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

internal class VirtualOpCodes : IRuntimeMutator
{
    
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        // issue at hand : we have no way of calculating the start key

        foreach (var chunk in rt.VmChunks.ToArray())
         foreach (var instr in chunk.Content.ToArray())
         {
 
             switch (instr.OpCode)
             {
                 case VmCode.Dup:

                     var endJmpLoc = new VmInstruction(VmCode.Ldc_I4);
                     var endJmpKey = new VmInstruction(VmCode.Ldc_I4);

                     var endJmp = new VmInstruction(VmCode.Jmp);
                     
                     var region = GetOrCreateHandlerRegion(instr.OpCode, rt);
                     
                     chunk.Content.ReplaceRange(instr, // push current location
                         endJmpLoc,
                         endJmpKey,
                         
                         new VmInstruction(VmCode.Ldc_I4, region), // push region loc
                         new VmInstruction(VmCode.Ldc_I4, rt.Descriptor.Data.GetStartKey(chunk)),
                         endJmp
                         );
                     
                     // now split
                     
                     var newChunk = chunk.Split(rt, chunk.Content.IndexOf(endJmp));

                     endJmpLoc.Operand = newChunk;
                     endJmpKey.Operand = rt.Descriptor.Data.GetStartKey(newChunk);
                     
                     break;
                 
                 default: // not a pseudocode
                     continue;
             }
 
         }
         

    }

    private VmChunk GetOrCreateHandlerRegion(VmCode code, VirtualGuardRT rt)
    {
        if (!_handlers.TryGetValue(code, out VmInstruction[] value))
            throw new InvalidOperationException("No virtual handler for pseudocode " + code);

        if (_codeChunkMap.TryGetValue(code, out VmChunk chunk))
            return chunk;
        
        // make new chunk

        var newChunk = new VmChunk(value.ToList());
        _codeChunkMap.Add(code, newChunk);
        
        // add to runtime
        rt.AddChunk(newChunk);

        return newChunk;
    }

    private Dictionary<VmCode, VmChunk> _codeChunkMap = new Dictionary<VmCode, VmChunk>();
    
    private Dictionary<VmCode, VmInstruction[]> _handlers = new Dictionary<VmCode, VmInstruction[]>()
    {
        {
            VmCode.Dup,
            new[]
            {
                new VmInstruction(VmCode.Stloc, (short)-3), // store return key
                new VmInstruction(VmCode.Stloc, (short)-2), // store return addr
                new VmInstruction(VmCode.Stloc, (short)-1),
                new VmInstruction(VmCode.Ldloc, (short)-1),
                new VmInstruction(VmCode.Ldloc, (short)-1),
                new VmInstruction(VmCode.Ldloc, (short)-2), // push return addr
                new VmInstruction(VmCode.Ldloc, (short)-3), // push return key
                new VmInstruction(VmCode.Jmp)
            }
        }
    };
    
}