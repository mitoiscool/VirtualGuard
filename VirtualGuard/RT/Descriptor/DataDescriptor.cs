using System.Reflection;
using AsmResolver.Patching;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.RT.Chunk;
using VirtualGuard.RT.Dynamic;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor;

public class DataDescriptor
{
    public DataDescriptor(Random rnd, int debugKey)
    { // debug
        byte[] randomBytes1 = new byte[5];
        byte[] randomBytes2 = new byte[5];
        byte[] randomBytes3 = new byte[5];
        
        rnd.NextBytes(randomBytes1);
        HandlerShifts = randomBytes1;
        
        rnd.NextBytes(randomBytes2);
        ByteShifts = randomBytes2;

        rnd.NextBytes(randomBytes3);
        HeaderRotationFactors = randomBytes3;
        
        InitialHeaderKey = (byte)_rnd.Next(255);

        DebugKey = debugKey;
        
        // populate vmcode fixup mutations

        for (int i = 0; i < typeof(VmCode).GetEnumNames().Where(x => x.Substring(0, 2) != "__").ToArray().Length; i++)
        { // for all existing opcodes generate mutation expression
            _fixupMutations.Add((VmCode)i, MutationExpression.Random());
        }

    }

    public byte InitialHeaderKey;
    public byte[] HeaderRotationFactors;

    public int DebugKey;
    
    private Dictionary<int, string> _stringMap = new Dictionary<int, string>();

    private Dictionary<VmChunk, byte> _chunkKeyMap = new Dictionary<VmChunk, byte>();

    private Dictionary<VmCode, MutationExpression> _fixupMutations = new Dictionary<VmCode, MutationExpression>();

    public byte EmulateFixupMutation(VmCode code, int fixup)
    {
        var mutation = _fixupMutations[code];

        return mutation.Solve(fixup);
    }

    public CilInstruction[] GetFixupMutationCil(VmCode code)
    {
        return _fixupMutations[code].ToCIL();
    }

    public byte GetStartKey(VmChunk chunk)
    {
        return _chunkKeyMap[chunk];
    }

    public void BuildStartKey(VmChunk chunk)
    {
        var startKey = (byte)_rnd.Next(255);
        _chunkKeyMap.Add(chunk, startKey);
    }

    public Dictionary<VmChunk, byte> GetExports()
    {
        return _chunkKeyMap;
    }

    public string StreamName;
    public string Watermark;
    
    public byte[] ByteShifts;
    public byte[] HandlerShifts;
        
    private static Random _rnd = new Random();
    public int AddString(string s)
    {
        int id = _rnd.Next(int.MaxValue);

        while(_stringMap.ContainsKey(id))
            id = _rnd.Next(int.MaxValue);
        
        _stringMap.Add(id, s);

        return id;
    }

    public Dictionary<int, string> GetStrings()
    {
        return _stringMap;
    }
    
}