using System.Data;
using System.Diagnostics;
using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.RT;
using VirtualGuard.RT.Descriptor;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard;

public static class Util
{
    internal static void Shuffle<T>(this Random random, IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    
    internal static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        var random = new Random();
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    
    internal static void ReplaceRange<T>(this List<T> list, T existingItem, params T[] newItems)
    { // chatgpt ftw
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (existingItem == null)
        {
            throw new ArgumentNullException(nameof(existingItem));
        }

        int index = list.IndexOf(existingItem);

        if (index == -1)
        {
            throw new ArgumentException("The specified item does not exist in the list.", nameof(existingItem));
        }

        // Remove the existing item
        list.RemoveAt(index);

        // Insert new items at the removed item's index
        list.InsertRange(index, newItems);
    }
    
    public static void ReplaceRange(this CilInstructionCollection list, CilInstruction existingItem, params CilInstruction[] newItems)
    { // chatgpt ftw
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (existingItem == null)
        {
            throw new ArgumentNullException(nameof(existingItem));
        }

        int index = list.IndexOf(existingItem);

        if (index == -1)
        {
            throw new ArgumentException("The specified item does not exist in the list.", nameof(existingItem));
        }

        var firstInstr = newItems[0];
        
        // Replace the existing item with the new code
        list[index++].ReplaceWith(firstInstr.OpCode, firstInstr.Operand);

        // Insert new items infront of the new item
        list.InsertRange(index, newItems.Except(new [] { firstInstr }));
    }

    public static TypeDefinition LookupType(this ModuleDefinition mod, string name)
    {
        return mod.GetAllTypes().Single(x => x.FullName == name);
    }
    
    internal static MethodDefinition LookupMethod(this ModuleDefinition mod, string name)
    {
        string[] split = name.Split(":");
        
        return mod.GetAllTypes().Single(x => x.FullName == split[0]).Methods.Single(x => x.Name == split[1]);
    }

    private static Random _rnd = new Random();
    
    public static TypeDefinition Clone(this TypeDefinition def, ModuleDefinition mod)
    {
        var cloner = new MemberCloner(mod);

        cloner.Include(def);

        var member = cloner.Clone().GetClonedMember(def);
        member.Name = "{" + _rnd.Next() + "}-" + member.Name;

        return member;
        
        var newType = new TypeDefinition(def.Namespace, def.Name + "_clone-" + _rnd.Next(), def.Attributes, mod.DefaultImporter.ImportType(def.BaseType));

        foreach (var interfac in def.Interfaces)
        {
            newType.Interfaces.Add(new InterfaceImplementation(interfac.Interface));
        }
        
        // clone methods

        if (def.Fields.Count > 0)
            throw new InvalidOperationException("could not clone type with fields");

        foreach (var impl in def.MethodImplementations)
            newType.MethodImplementations.Add(new MethodImplementation(mod.DefaultImporter.ImportMethodOrNull(impl.Declaration), mod.DefaultImporter.ImportMethodOrNull(impl.Body)));

        foreach (var oldMeth in def.Methods)
        {
            // clone locals
            var newMeth = new MethodDefinition(oldMeth.Name, oldMeth.Attributes, oldMeth.Signature);

            if(oldMeth.CilMethodBody == null)
                continue;
            
            newMeth.CilMethodBody = new CilMethodBody(newMeth);

            foreach (var var in oldMeth.CilMethodBody.LocalVariables)
                newMeth.CilMethodBody.LocalVariables.Add(new CilLocalVariable(var.VariableType));

            var body = oldMeth.CilMethodBody;
            var clonedBody = newMeth.CilMethodBody;

            foreach (var instr in body.Instructions)
                clonedBody.Instructions.Add(instr);

            foreach (var instr in clonedBody.Instructions)
            {
                if (instr.Operand is CilLocalVariable v)
                    instr.Operand = clonedBody.LocalVariables[v.Index];

                if (instr.IsBranch())
                    instr.Operand = new CilInstructionLabel(clonedBody.Instructions[body.Instructions.IndexOf(((CilInstructionLabel)instr.Operand).Instruction)]); // localize
            }
            
            newType.Methods.Add(newMeth);
        }

        return newType;
    }
    
    

    public static IMemberDefinition LookupMember(this ModuleDefinition mod, string name)
    {
        string[] split = name.Split(":");
        var targetType = mod.LookupType(split[0]);

        var members = new List<IMemberDefinition>();
        
        members.AddRange(targetType.Methods);
        members.AddRange(targetType.Fields);
        
        return members.Single(x => x.Name == split[1]);
    }

    internal static int HashNumber(int number, HashDescriptor hs)
    {
        // Perform bit-shifting and arithmetic operations for mutation
        int mutatedNumber = ((number << hs.NSalt1) + hs.NSalt2) ^ hs.NSalt3;

        // Ensure the mutated number is non-negative
        int absoluteMutatedNumber = Math.Abs(mutatedNumber);

        // Perform the hashing operation
        int hashedValue = ((absoluteMutatedNumber * hs.NKey) % 1000) + hs.NSalt3; // Modulus to keep the result within a reasonable range

        return hashedValue;
    }


    internal static VmInstruction[] BuildHashInstructions(AstExpression comparer, VmMethod ctx, VirtualGuardRT rt)
    {
        //return Array.Empty<VmInstruction>();
        
        var instrs = new List<VmInstruction>();
        
        // ok this will be a pain in the ass but it's ok

        // todo: have each type have their own hashing function (fml)

        // first, determine which variable has the constant
        Debug.Assert(comparer.Arguments.Length == 2);

        VmInstruction constantParent = null;
        bool shouldReverseStack = comparer.Arguments[0].OpCode.IsHashableConstant();

        if (comparer.Arguments[0].OpCode.IsHashableConstant())
            constantParent = ctx.GetTranslatedInstructions(comparer.Arguments[0]).Last(); // pray this is only 1, should probably debug assert
        
        if (comparer.Arguments[1].OpCode.IsHashableConstant())
            constantParent = ctx.GetTranslatedInstructions(comparer.Arguments[0]).Last();

        if (constantParent == null)
            return Array.Empty<VmInstruction>(); // don't use any instrs, no need to hash

        // now we have constant, let's hash

        // look into reversing stack order

        if (shouldReverseStack)
        {
            var holder = ctx.GetTempVar();

            instrs.AddRange( new[] {
                new VmInstruction(VmCode.Stloc, holder),
                new VmInstruction(VmCode.Hash),
                new VmInstruction(VmCode.Ldloc, holder)
                }
            );
        }
        else
        {
            // else just insert hash
            instrs.Add(new VmInstruction(VmCode.Hash));
        }

        // do specifics according to hash req

        switch (constantParent.OpCode)
        {
            case VmCode.Ldc_I4:
                // hash int, also ensure it's not a branch or local (shouldn't be)
                Debug.Assert(constantParent.Operand is int);

                Console.WriteLine("hashed constant parent: " + constantParent.ToString());
                
                constantParent.OpCode = VmCode.Ldc_I8; // first time using this code lol
                constantParent.Operand = ComputeHash(BitConverter.GetBytes((int)constantParent.Operand), rt.Descriptor.HashDescriptor);
                break;
            
            case VmCode.Ldc_I8:
                constantParent.OpCode = VmCode.Ldc_I8; // first time using this code lol
                constantParent.Operand = ComputeHash(BitConverter.GetBytes((long)constantParent.Operand), rt.Descriptor.HashDescriptor);
                break;
            
            case VmCode.Ldstr:
                constantParent.OpCode = VmCode.Ldc_I8; // first time using this code lol
                constantParent.Operand = ComputeHash(Encoding.ASCII.GetBytes((string)constantParent.Operand), rt.Descriptor.HashDescriptor);
                break;
        }
        

        return instrs.ToArray();
    }
    
    static bool IsHashableConstant(this CilOpCode code)
    {
        return new[]
        {
            CilCode.Ldc_I4,
            CilCode.Ldc_I8,
            
            CilCode.Ldstr
        }.Contains(code.Code);
    }
    
    internal static long ComputeHash(byte[] buffer, HashDescriptor hd)
    {
        uint[] table = new uint[256];

        for (uint i = 0; i < 256; i++)
        {
            uint entry = i;
            for (int j = 0; j < 8; j++)
            {
                entry = (entry & 1) == 1
                    ? (entry >> 1) ^ hd.SPolynomial
                    : entry >> 1;

                entry ^= (entry >> 12) ^ (entry >> 24);
            }

            table[i] = entry;
        }

        uint hash = hd.SSeed;

        for (int i = 0; i < buffer.Length; i++)
        {
            hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
        }

        return ~hash ^ hd.SXorMask;
    }
    
}