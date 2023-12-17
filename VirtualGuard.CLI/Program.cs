// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Builder;
using AsmResolver.PE.DotNet.Builder;
using VirtualGuard;
using VirtualGuard.CLI;
using VirtualGuard.CLI.Config;
using VirtualGuard.CLI.Processors;
using VirtualGuard.CLI.Processors.impl;

// arg format <path> <output_path>

if (args.Length != 4)
    throw new ArgumentException("Expected 4 args.");

string path = args[0];
string outputPath = args[1];
string settingsPath = args[2];

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
    
    File.WriteAllText("config.json", JsonSerializer.Serialize(cfg));
}

int debugKey = int.Parse(args[3]);

var processors = new IProcessor[]
{
    new DataEncryption(),
    
    new Virtualization(), // populates vm elements
    
    
};

var ctx = new Context(module, JsonSerializer.Deserialize<SerializedConfig>(File.ReadAllText(settingsPath)));
var logger = new ConsoleLogger();

ctx.Virtualizer = new Virtualizer(new VirtualGuardContext(module, logger), debugKey);

foreach (var processor in processors)
{
    processor.Process(ctx);
    logger.LogSuccess("Processed: " + processor.Identifier);
}

// save file
ctx.Module.Write(outputPath, new ManagedPEImageBuilder(MetadataBuilderFlags.PreserveTableIndices));
