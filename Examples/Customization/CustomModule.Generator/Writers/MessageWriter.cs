using CustomModule.Commands;
using KY.Core;
using KY.Generator;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace CustomModule.Generator.Writers;

/// <summary>
/// Builds the output as a tree of templates rather than as text, so the same tree can be written by any
/// language writer. Deriving from Codeable is what provides the <c>Code</c> helper.
/// </summary>
internal class MessageWriter(IList<FileTemplate> files, Options options) : Codeable
{
    public void Write(WriteMessageCommandParameters parameters)
    {
        GeneratorOptions generatorOptions = options.Get<GeneratorOptions>();
        string className = parameters.ClassName ?? "Greeter";
        Logger.Trace($"Write {className}...");

        FileTemplate file = files.AddFile(parameters.RelativePath, generatorOptions);
        // Naming the file is the writer's job. Formatter asks the language set in Prepare() how a file is
        // named, so this comes out as Greeter.cs for C# - including the extension. Leave it out and the
        // file is written without one.
        file.Name = Formatter.FormatFile(className, generatorOptions);

        file.AddNamespace(parameters.Namespace ?? string.Empty)
            .AddClass(className)
            .AddMethod("SayHello", Code.Void())
            .Static()
            // System.Console rather than a using plus Console - one less thing for the writer to track.
            .Code.AddLine(Code.Static(Code.Type("System.Console"))
                              .Method("WriteLine", Code.String(parameters.Message ?? string.Empty))
                              .Close());
    }
}
