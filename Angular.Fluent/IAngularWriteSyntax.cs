namespace KY.Generator;

public interface IAngularWriteSyntax : IAngularWriteBaseSyntax<IAngularWriteSyntax>
{
    /// <summary>
    /// Creates the code for a npm package
    /// </summary>
    IAngularWriteSyntax Package(Action<IAngularPackageSyntax> action);
}
