// See https://aka.ms/new-console-template for more information

using AsmResolver.DotNet;
using VirtualGuard;
using VirtualGuard.CLI.VG;

var ctx = new VirtualGuardContext(ModuleDefinition.FromFile("VirtualGuard.TestBinary.dll"), new VirtualGuardSettings()
{
    DebugMessageKey = 0, // makes it not encrypted
    License = LicenseType.Plus,
    Version = "v1.0"
});

var virt = new Virtualizer(ctx);

virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "AddTest"), true);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "CallTest"), true);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "AdvancedTest"), true);

virt.CommitRuntime();

ctx.Module.Write("VirtualGuard.TestBinary.dll");