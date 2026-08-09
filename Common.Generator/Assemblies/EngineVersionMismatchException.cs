namespace KY.Generator;

/// <summary>
/// Thrown by the <see cref="EngineVersionGuard" /> when a second engine version is about to enter the process.
/// </summary>
public class EngineVersionMismatchException : Exception
{
    public EngineVersionMismatchException(string message)
        : base(message)
    { }
}
