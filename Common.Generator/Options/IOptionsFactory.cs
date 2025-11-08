namespace KY.Generator;

public interface IOptionsFactory
{
    bool CanCreate(Type optionsType);
    object Create(Type optionsType, object key, object? parent, object global);
    object CreateGlobal(Type optionsType, object key, object? parent);
}