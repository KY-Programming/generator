namespace KY.Generator;

public interface IAspDotNetReadSyntax
{
    /// <summary>
    /// Read all metadata from this controller and provide it to the following write actions.
    /// This method does not generate anything. You need at least on write action. Use <code>.Write()</code> method.
    /// </summary>
    /// <example>
    /// <code>
    /// this.Read()
    ///  .FromController&lt;MyTestController&gt;()
    ///  .Write()
    ///  .AngularModels().OutputPath("RelativePath/Models").NoHeader()
    ///  .AngularServices().OutputPath("RelativePath/Services").NoHeader();
    /// </code>
    /// </example>
    /// <typeparam name="T">Type which should be generated</typeparam>
    IAspDotNetReadSyntax FromController<T>();

    /// <summary>
    /// Read all metadata from this SignalR hub and provide it to the following write actions.
    /// This method does not generate anything. You need at least on write action. Use <code>.Write()</code> method.
    /// </summary>
    /// <example>
    /// <code>
    /// this.Read()
    ///  .FromHub&lt;MyTestHub&gt;()
    ///  .Write()
    ///  .AngularModels().OutputPath("RelativePath/Models").NoHeader()
    ///  .AngularServices().OutputPath("RelativePath/Services").NoHeader();
    /// </code>
    /// </example>
    /// <typeparam name="T">Type which should be generated</typeparam>
    IAspDotNetReadSyntax FromHub<T>();
}
