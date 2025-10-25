namespace KY.Generator.Angular;

public interface IAngularWriteBaseSyntax<out T>
{
    /// <summary>
    /// Writes all models (POCOs) from all previous read actions to the output, with some angular specific modifications
    /// </summary>
    T Models(Action<IAngularModelSyntax> action = null);

    /// <summary>
    /// Writes all services from all previous read actions to the output
    /// </summary>
    T Services(Action<IAngularServiceSyntax> action = null);
}
