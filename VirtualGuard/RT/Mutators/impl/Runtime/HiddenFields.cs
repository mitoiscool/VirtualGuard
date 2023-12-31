using System.Diagnostics;
using System.Drawing;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Collections;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Echo.Platforms.AsmResolver;

namespace VirtualGuard.RT.Mutators.impl.Runtime;

internal class HiddenFields : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        
        // scary
        // in non-static types, we need to create a nested type that has an object[]
        // typeof this field will be an instance field, which once prepared will be used to replace instance field refs

        var fieldCollectionType = new TypeDefinition("", "FieldCollection", TypeAttributes.Public);
        
        rt.RuntimeModule.TopLevelTypes.Add(fieldCollectionType);

        var fieldCollectionCtor = MethodDefinition.CreateConstructor(rt.RuntimeModule, rt.RuntimeModule.CorLibTypeFactory.Int32);

        fieldCollectionType.Methods.Add(fieldCollectionCtor);

        var fieldCollection = new FieldDefinition("collection", FieldAttributes.Public,
            new SzArrayTypeSignature(rt.RuntimeModule.CorLibTypeFactory.Object));
        
        fieldCollectionType.Fields.Add(fieldCollection);

        var factory = rt.RuntimeModule.CorLibTypeFactory;
        
        var objectCtor = factory.CorLibScope
            .CreateTypeReference("System", "Object")
            .CreateMemberReference(".ctor", MethodSignature.CreateInstance(
                factory.Void))
            .ImportWith(rt.RuntimeModule.DefaultImporter);
        
        fieldCollectionCtor.CilMethodBody = new CilMethodBody(fieldCollectionCtor);
        var instrs = fieldCollectionCtor.CilMethodBody.Instructions;
        
        instrs.Add(CilOpCodes.Ldarg_0);

        instrs.Add(CilOpCodes.Ldarg_1);
        instrs.Add(CilOpCodes.Newarr, rt.RuntimeModule.CorLibTypeFactory.Object.ToTypeDefOrRef());
        
        instrs.Add(CilOpCodes.Stfld, fieldCollection);
        
        instrs.Add(CilOpCodes.Ldarg_0);
        
        instrs.Add(CilOpCodes.Call, objectCtor);
        
        instrs.Add(CilOpCodes.Ret);

        var importedCollection = rt.RuntimeModule.DefaultImporter.ImportType(fieldCollectionType);

        var importedCtorSig = rt.RuntimeModule.DefaultImporter.ImportMethodSignature(fieldCollectionCtor.Signature);
        var importedCtor = importedCollection.CreateMemberReference(".ctor", importedCtorSig);

        var collectionSig = rt.RuntimeModule.DefaultImporter.ImportTypeSignature(importedCollection.ToTypeSignature());
        
        
        foreach (var type in rt.RuntimeModule.GetAllTypes().Where(x => !x.IsModuleType && x.Fields.Count > 0 && x != fieldCollectionType))
        {

            var instanceFields = type.Fields.Where(x => !x.IsStatic && x.IsPrivate).ToArray();
            instanceFields.Shuffle();

            if(instanceFields.Length == 0)
                continue;

            var fieldCount = instanceFields.Length;
            var fieldIndexMap = new Dictionary<IFieldDescriptor, int>();

            for(int i = 0; i < fieldCount; i++)
                fieldIndexMap.Add(instanceFields[i], i);

            // add field to hold collection
            var holder = new FieldDefinition("holder", FieldAttributes.Public, collectionSig);
            type.Fields.Add(holder);
            
            // initialize

            var ctor = type.Methods.Where(x => x.Name == ".ctor" && x.CilMethodBody != null).ToList();

            foreach (var ct in ctor)
            { // add init
                ct.CilMethodBody.Instructions.InsertRange(ct.CilMethodBody.Instructions[1].Operand is IMethodDescriptor descriptor && descriptor.FullName == objectCtor.FullName ? 2 : 0, new [] // insert at 2 cuz object ctor
                {
                    new CilInstruction(CilOpCodes.Ldarg_0), // inst
                    new CilInstruction(CilOpCodes.Ldc_I4, fieldCount),
                    new CilInstruction(CilOpCodes.Newobj, fieldCollectionCtor),
                    new CilInstruction(CilOpCodes.Stfld, holder)
                });
            }

            if (ctor.Count == 0)
            {
                var newCtor = MethodDefinition.CreateConstructor(rt.RuntimeModule);
                newCtor.CilMethodBody = new CilMethodBody(newCtor);
                
                newCtor.CilMethodBody.Instructions.AddRange(new []
                {
                    new CilInstruction(CilOpCodes.Ldarg_0), // inst
                    new CilInstruction(CilOpCodes.Ldc_I4, fieldCount),
                    new CilInstruction(CilOpCodes.Newobj, importedCtor),
                    new CilInstruction(CilOpCodes.Stfld, holder)
                });
                
                newCtor.CilMethodBody.Instructions.Add(CilOpCodes.Ldarg_0);
        
                newCtor.CilMethodBody.Instructions.Add(CilOpCodes.Call, objectCtor);
            }

            foreach (var meth in type.Methods.Where(x => x.CilMethodBody != null))
            {
                meth.CilMethodBody.Instructions.ExpandMacros();
                
                foreach (var instr in meth.CilMethodBody.Instructions.ToArray())
                {
                    if(instr.OpCode == CilOpCodes.Ldarg)
                        Console.WriteLine(instr.Operand.GetType());
                    
                    if (instr.OpCode == CilOpCodes.Stfld)
                    {
                        if (instr.Operand is IFieldDescriptor fd && fd != holder && fd.DeclaringType == type)
                        {
                            var index = meth.CilMethodBody.Instructions.IndexOf(instr); // bc original is array

                            var arrayLoc = fieldIndexMap[fd];
                            
                            meth.CilMethodBody.Instructions.RemoveAt(index);

                            var loc = new CilLocalVariable(fd.Signature.FieldType);
                            meth.CilMethodBody.LocalVariables.Add(loc);
                            

                            meth.CilMethodBody.Instructions.InsertRange(index, new []
                            {
                                new CilInstruction(CilOpCodes.Stloc, loc),
                                
                                new CilInstruction(CilOpCodes.Ldfld, holder),
                                new CilInstruction(CilOpCodes.Ldc_I4, arrayLoc),
                                new CilInstruction(CilOpCodes.Ldloc, loc),

                                loc.VariableType.IsValueType ? new CilInstruction(CilOpCodes.Box, 
                                    // determine if arg, use arg type
                                    meth.CilMethodBody.Instructions[index - 1].Operand is Parameter param ? param.ParameterType.ToTypeDefOrRef() : loc.VariableType.ToTypeDefOrRef()
                                ) : new CilInstruction(CilOpCodes.Nop),

                                new CilInstruction(CilOpCodes.Stelem, rt.RuntimeModule.CorLibTypeFactory.Object.ToTypeDefOrRef())
                            });
                        }
                    }

                    if (instr.OpCode == CilOpCodes.Ldfld)
                    {
                        if (instr.Operand is IFieldDescriptor fd && fd != holder && fd.DeclaringType == type)
                        {
                            var index = meth.CilMethodBody.Instructions.IndexOf(instr); // bc original is array

                            var arrayLoc = fieldIndexMap[fd];
                            
                            meth.CilMethodBody.Instructions.RemoveAt(index);

                            meth.CilMethodBody.Instructions.InsertRange(index, new []
                            {
                                new CilInstruction(CilOpCodes.Ldfld, holder),
                                new CilInstruction(CilOpCodes.Ldc_I4, arrayLoc),

                                new CilInstruction(CilOpCodes.Ldelem, rt.RuntimeModule.CorLibTypeFactory.Object.ToTypeDefOrRef()),
                                new CilInstruction(CilOpCodes.Unbox_Any, fd.Signature.FieldType.ToTypeDefOrRef())
                            });
                        }
                    }
                    
                }
                
                meth.CilMethodBody.Instructions.CalculateOffsets();
                meth.CilMethodBody.Instructions.OptimizeMacros();
            }
            
            foreach (var field in instanceFields)
            {
                type.Fields.Remove(field);
            }
        }
        
        
        
    }
    
    
}