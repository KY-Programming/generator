using System.Reactive.Linq;
using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCasesAnnotations;

/// <summary>
/// The type exposes a member whose type comes from a NuGet package rather than from the project or the
/// framework, so the assembly loader has to resolve System.Reactive out of the package folder before the
/// type can be read at all.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class TypeFromNugetPackage
{
    public string Test { get; set; } = "";

    public IObservable<int> Count()
    {
        return Observable.Create<int>(observer =>
        {
            observer.OnNext(1);
            observer.OnCompleted();
            return () => { };
        });
    }
}
