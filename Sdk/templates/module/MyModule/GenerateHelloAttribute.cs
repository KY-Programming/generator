using KY.Generator;

namespace MyModule;

/// <summary>
/// The annotation entry point: a user of this module writes [GenerateHello("...")] on a class and the
/// generator runs during their build.
/// <para>
/// An annotation is nothing but a recipe for command line commands. $NAME$ and $NAMESPACE$ are replaced
/// with the name and namespace of the annotated type, so the command below is the same one a user could
/// type by hand.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class GenerateHelloAttribute : Attribute, IGeneratorCommandAttribute
{
    /// <summary>The text the generated class will write to the console.</summary>
    public string Message { get; }

    /// <summary>Output folder, relative to the project. Defaults to the project root.</summary>
    public string? RelativePath { get; set; }

    /// <param name="message">The text the generated class will write to the console.</param>
    public GenerateHelloAttribute(string message)
    {
        this.Message = message;
    }

    /// <summary>The commands this annotation stands for. Called by the generator, not by your code.</summary>
    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("say-hello", this.Parameters)
    ];

    private List<string> Parameters
    {
        get
        {
            List<string> parameters =
            [
                $"-message={this.Message}",
                "-className=$NAME$Greeter",
                "-namespace=$NAMESPACE$"
            ];
            if (this.RelativePath != null)
            {
                parameters.Add($"-relativePath={this.RelativePath}");
            }
            return parameters;
        }
    }
}
