// See https://aka.ms/new-console-template for more information

using AsmResolver.DotNet;
using AsmResolver.DotNet.Builder;
using AsmResolver.PE.DotNet.Builder;
using VirtualGuard;
using VirtualGuard.CLI.VG;

var ctx = new VirtualGuardContext(ModuleDefinition.FromFile("VirtualGuard.Tests.exe"), new VirtualGuardSettings()
{
    DebugMessageKey = 121, // makes it not encrypted
    License = LicenseType.Plus,
    Version = "v1.0"
});

var virt = new Virtualizer(ctx);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "AddTest"), true);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "CallTest"), true);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "AdvancedTest"), true);

virt.AddMethod(ctx.Module.LookupMethod("VirtualGuard.Tests.MSILExample:BranchingInstructions"), true);
virt.AddMethod(ctx.Module.LookupMethod("VirtualGuard.Tests.MSILExample:LoopingInstructions"), true);

virt.CommitRuntime();


ctx.Module.Write("VirtualGuard.Tests-virt.exe", new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveAll));