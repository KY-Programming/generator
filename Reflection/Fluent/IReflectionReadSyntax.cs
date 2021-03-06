﻿using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    public interface IReflectionReadSyntax : ISwitchToWriteSyntax
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
        ///  .AngularModels().OutputPath("Output/Models").SkipHeader()
        ///  .AngularServices().OutputPath("Output/Services").SkipHeader();
        /// </code>
        /// </example>
        /// <typeparam name="T">Type which should be generated</typeparam>
        IReflectionFromTypeOrReflectionReadSyntax FromType<T>();
    }
}