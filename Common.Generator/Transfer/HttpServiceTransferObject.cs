using KY.Generator.Languages;

namespace KY.Generator.Transfer;

public class HttpServiceTransferObject : TypeTransferObject
{
    public string? Route { get; set; }
    public string? Version { get; set; }
    public ILanguage? Language { get; set; }
    public List<HttpServiceActionTransferObject> Actions { get; } = [];
}
