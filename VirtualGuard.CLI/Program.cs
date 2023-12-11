// See https://aka.ms/new-console-template for more information

using AsmResolver.DotNet;
using VirtualGuard;
using VirtualGuard.CLI.VG;

var ctx = new VirtualGuardContext(ModuleDefinition.FromFile("VirtualGuard.TestBinary.dll"));
var virt = new Virtualizer(ctx);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "AddTest"), true);

//virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "CallTest"), true);

virt.AddMethod(ctx.Module.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == "ConditionalTest"), true);

virt.CommitRuntime();

ctx.Module.Write("out.exe");