namespace KY.Generator.Reflection.Fluent
{
    public interface IReflectionFromTypeSyntax
    {
        /// <summary>
        /// Reads only the metadata from used/referenced types.
        /// Use this method to create a index type, which will not be generated, but adds all used types to generation process
        /// </summary>
        IReflectionReadSyntax SkipSelf();
    }
}