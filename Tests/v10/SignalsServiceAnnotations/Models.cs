using KY.Generator;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace SignalsService;

/// <summary>
/// Model with signals. The generated service has to wrap it after every read and unwrap it before every write
/// </summary>
[GenerateWithSignals]
public class SignalModel
{
    public string Text { get; set; } = string.Empty;
    public int Number { get; set; }
    public DateTime Timestamp { get; set; }
    public string? OptionalText { get; set; }
    public List<string> Texts { get; set; } = [];
    public SubModel Sub { get; set; } = new();
    public List<SubModel> Subs { get; set; } = [];
}

/// <summary>
/// Nested model. Inherits the signals from the model that uses it and gets its own wrap/unwrap methods
/// </summary>
public class SubModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime Changed { get; set; }
}

/// <summary>
/// Model without signals. Has to be read and written completely unchanged
/// </summary>
public class PlainModel
{
    public string Name { get; set; } = string.Empty;
}
