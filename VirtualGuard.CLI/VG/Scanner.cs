using AsmResolver.DotNet;

namespace VirtualGuard.CLI.VG;

public class Scanner
{
    private ModuleDefinition _module;
    private Context _ctx;
    private VirtualGuardContext _vgCtx;

    // determine if methods should be exports or not
    public Scanner(ModuleDefinition mod, Context ctx, VirtualGuardContext vgCtx)
    {
        _module = mod;
        _ctx = ctx;
        _vgCtx = vgCtx;
    }


    public bool ShouldExport(MethodDefinition def)
    {
        // look for references to this (this has to be a horrible way of doing this)
        // this won't work when people use reflection (ugh)

        if (def.IsConstructor)
            return true;
        
        foreach (var method in _module.GetAllTypes().SelectMany(x => x.Methods).Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 0))
        {
            
            foreach (var instr in method.CilMethodBody.Instructions.Where(x => x.Operand is IMemberDefinition id && id.FullName == def.FullName))
            {
                if (_ctx.methodExports.ContainsKey(method))
                    continue; // reference is from vm, so it should still not necessarily be an export
                
                
                Console.WriteLine("found reference to {0} in {1}", def.Name, method.Name);
            }
            
        }
        
        
        return false;
    }
}