﻿using System.Reflection.Emit;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT;

public class RuntimeProcessor
{
    
    public RuntimeProcessor(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        _runtime = rt;
        _ctx = ctx;
    }
    
    private VirtualGuardRT _runtime;
    private VirtualGuardContext _ctx;

    public void FinalizeMethods()
    {
        foreach (var virtualizedMethodKvp in _ctx.VirtualizedMethods)
        {
            virtualizedMethodKvp.Deconstruct(out MethodDefinition meth, out bool export);
            
            if(export)
                PatchMethod(meth);
            else
                RemoveMethod(meth);
            
            
        }
        
        
        
    }


    void PatchMethod(MethodDefinition meth)
    {
        var loc = _runtime.GetExportLocation(meth);

        var instrs = meth.CilMethodBody.Instructions;
        
        
        instrs.Clear();
        
        // VMEntry(loc, args)
        
        instrs.Add(CilOpCodes.Ldc_I4, loc);
        
        // ldc.i4 paramcount
        // newarr <object>
        
        // iterate:
        
        // dup
        // ldc.i4 index
        
        // ldarg param
        
        // box param.type
        // stelem.ref
        
        
        instrs.Add(CilOpCodes.Ldc_I4, meth.Parameters.Count);
        instrs.Add(CilOpCodes.Newarr, _ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef());

        int index = 0;
        foreach (var param in meth.Parameters)
        {
            instrs.Add(CilOpCodes.Dup);
            instrs.Add(CilOpCodes.Ldc_I4, index);
            
            instrs.Add(CilOpCodes.Ldarg, param);
            
            instrs.Add(CilOpCodes.Box, param.ParameterType.ToTypeDefOrRef()); // get paramtype
            instrs.Add(CilOpCodes.Stelem_Ref);
            
            index++;
        }
        
        
        instrs.Add(CilOpCodes.Call, _runtime.Elements.VmEntry);

        if (!meth.Signature.ReturnsValue)
        {
            instrs.Add(CilOpCodes.Pop);
        }
        else
        {
            instrs.Add(CilOpCodes.Unbox_Any, meth.Signature.ReturnType.ToTypeDefOrRef());
        }
            
        
        instrs.Add(CilOpCodes.Ret);
        
        meth.CilMethodBody.VerifyLabels();
        meth.CilMethodBody.ComputeMaxStack();
    }

    void RemoveMethod(MethodDefinition def)
    {
        
    }
    
    
}