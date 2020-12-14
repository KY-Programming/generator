namespace KY.Generator.Reflection.Fluent
{
    public interface IReflectionReadSyntax
    {
        IReflectionReadAndSwitchToWriteSyntax FromType<T>();
    }
}