using System;

namespace KY.Generator.Reflection.Fluent
{
    public interface IReflectionReadSyntax
    {
        /// <summary>
        /// Read all metadata from this type and provide it to the following write actions.
        /// This method does not generate anything. You need at least on write action. Use <code>.Write()</code> method.
        /// </summary>
        /// <example>
        /// <code>
        /// this.Read()
        ///  .FromType&lt;MyClass&gt;()
        ///  .Write()
        ///  .AngularModels().OutputPath("Output/Models").NoHeader()
        ///  .AngularServices().OutputPath("Output/Services").NoHeader();
        /// </code>
        /// </example>
        /// <typeparam name="T">Type which should be generated</typeparam>
        IReflectionReadSyntax FromType<T>(Action<IReflectionFromTypeSyntax> options = null);
    }
}
