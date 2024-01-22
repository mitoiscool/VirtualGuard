// See https://aka.ms/new-console-template for more information

using System.Net;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Builder;
using AsmResolver.PE.DotNet.Builder;
using Newtonsoft.Json;
using VirtualGuard;
using VirtualGuard.CLI;
using VirtualGuard.CLI.Config;
using VirtualGuard.CLI.Processors;
using VirtualGuard.CLI.Processors.impl;

#if DEBUG
var debug = true;
#else
var debug = false
#endif

/*
 cli.exe -genconfig <path>
returns: gonna edit an example here

default cli:
 
cli.exe <license> <path> <output_path> <debug_key> -raw <raw config>
cli.exe <license> <path> <output_path> <debug_key> <cfg path>*/

if (args[0] == "-genconfig")
{
    var gen = new ConfigGenerator(ModuleDefinition.FromFile(args[1]));
    gen.Populate();
    
    Console.WriteLine(gen.Serialize());
    return;
}

LicenseType license = (LicenseType)int.Parse(args[0]); // will throw error if not int
string path = args[1];
string outputPath = args[2];
int debugKey = int.Parse(args[3]);

string settings = args[4] == "-raw" ? args[5] : File.ReadAllText(args[4]);

var logger = new ConsoleLogger();

var module = ModuleDefinition.FromFile(path);


// note: license is not initialized as of 12/17/23
var ctx = new Context(module, JsonConvert.DeserializeObject<SerializedConfig>(settings), logger, license);

var vgCtx = new VirtualGuardContext(module, logger);

// init processors
ctx.Virtualizer = new MultiProcessorVirtualizer(vgCtx, debug, debugKey, MultiProcessorAllocationMode.Sequential, ctx.Configuration.ProcessorCount);

var pipeline = new Queue<IProcessor>();

if (ctx.License == LicenseType.Free)
{
    pipeline.Enqueue(new FreeLimitations());
    ctx.Logger.Warning("License is free, therefore adding limitations.");
}

if (ctx.Configuration.UseDataEncryption)
{
    pipeline.Enqueue(new DataEncryption());
}

if (ctx.Configuration.UseImportProtection && license != LicenseType.Free)
{
    pipeline.Enqueue(new ImportProtection());
}

pipeline.Enqueue(new Virtualization());

if (ctx.Configuration.UseDataEncryption)
{
    pipeline.Enqueue(new PostDataEncryption()); // fml
}

//pipeline.Enqueue(new PostImportProtection());

if(ctx.Configuration.RenameDebugSymbols)
    pipeline.Enqueue(new Renamer());

pipeline.Enqueue(new Watermark());

while(pipeline.TryDequeue(out IProcessor processor)) {
    processor.Process(ctx);
    logger.Success("Processed: " + processor.Identifier);
}

// save file
ctx.Module.Write(outputPath, new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveTableIndices));

ctx.Logger.Success("Wrote file at " + outputPath);
