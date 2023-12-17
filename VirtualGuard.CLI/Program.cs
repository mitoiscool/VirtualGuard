// See https://aka.ms/new-console-template for more information

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
int debugKey = 0; // debug

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
    logger.LogFatal("Couldn't find config, created example @ config.json.");
}



var processors = new IProcessor[]
{
    new DataEncryption(),
    
    new Virtualization(), // populates vm elements
    
    
};

var ctx = new Context(module, JsonConvert.DeserializeObject<SerializedConfig>(File.ReadAllText(settingsPath)), logger);

ctx.Virtualizer = new Virtualizer(new VirtualGuardContext(module, logger), debugKey, false);

foreach (var processor in processors)
{
    processor.Process(ctx);
    logger.LogSuccess("Processed: " + processor.Identifier);
}

// save file
ctx.Module.Write(outputPath, new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveTableIndices));

ctx.Logger.LogSuccess("Wrote file at " + outputPath);
