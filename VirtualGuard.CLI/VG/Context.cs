using AsmResolver.DotNet;

namespace VirtualGuard.CLI.VG;

public class Context
{
    public Dictionary<MethodDefinition, bool> methodExports = new Dictionary<MethodDefinition, bool>();
}