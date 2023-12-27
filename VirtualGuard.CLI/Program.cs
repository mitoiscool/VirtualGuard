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
using JsonSerializer = System.Text.Json.JsonSerializer;

#if DEBUG
var debug = true;
var debugKey = 0;
#else
var debug = false;
var debugKey = new Random().Next(); // we should grab this from input args
#endif


// arg format <path> <output_path>

//if (args.Length != 4)
//    throw new ArgumentException("Expected 4 args.");

//string path = args[0];
//string outputPath = args[1];
//string settingsPath = args[2];
//int debugKey = int.Parse(args[3]);

string path = "VirtualGuard.Tests.exe";
string outputPath = "VirtualGuard.Tests-virt.exe";
string settingsPath = "config.json";
var license = LicenseType.Plus;

var logger = new ConsoleLogger();

var module = ModuleDefinition.FromFile(path);

if (!File.Exists(settingsPath))
{
    var cfg = new SerializedConfig()
    {
        UseDataEncryption = true,
        RenameDebugSymbols = true,
        Members = new []
        {
            new SerializedMember()
            {
                Exclude = true,
                Virtualize = false,
                Member = "TestNamespace.TestClass:TestMethod"
            },
            new SerializedMember()
            {
                Exclude = true,
                Virtualize = false,
                Member = "TestNameSpace.TestClass"
            }
        }
    };
    
    File.WriteAllText("config.json", JsonConvert.SerializeObject(cfg, Formatting.Indented));
    logger.Fatal("Couldn't find config, created example @ config.json.");
}



var processors = new IProcessor[]
{
    new DataEncryption(),
    
    new Virtualization(), // populates vm elements
    new Watermark()
};

#if DEBUG
Console.WriteLine("DEBUG");
#endif

// note: license is not initialized as of 12/17/23
var ctx = new Context(module, JsonConvert.DeserializeObject<SerializedConfig>(File.ReadAllText(settingsPath)), logger, license);

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

pipeline.Enqueue(new Virtualization());

foreach (var processor in processors)
{
    processor.Process(ctx);
    logger.Success("Processed: " + processor.Identifier);
}

// save file
ctx.Module.Write(outputPath, new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveTableIndices));

ctx.Logger.Success("Wrote file at " + outputPath);
