using KY.Generator;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace EdgeCases.Signals;

/// <summary>
/// Every member has to be generated as signal. Optional members stay optional in the value type, but the member itself
/// is always present e.g. <code>optionalString: WritableSignal&lt;string | undefined&gt;</code>
/// </summary>
[GenerateAngularModel("Output/Signals")]
[GenerateWithSignals, GeneratePreferInterfaces]
public class SignalInterface
{
    public string Text { get; set; } = string.Empty;
    public int Number { get; set; }
    public bool Switch { get; set; }
    public DateTime Timestamp { get; set; }
    public string? OptionalText { get; set; }
    public List<string> Texts { get; set; } = [];
}

/// <summary>
/// Every member has to be generated as signal. Optional members stay optional in the value type, but the member itself
/// is always present e.g. <code>optionalString: WritableSignal&lt;string | undefined&gt;</code>
/// </summary>
[GenerateAngularModel("Output/Signals")]
[GenerateWithSignals]
public class SignalClass
{
    public string Text { get; set; } = string.Empty;
    public int Number { get; set; }
    public bool Switch { get; set; }
    public DateTime Timestamp { get; set; }
    public string? OptionalText { get; set; }
    public List<string> Texts { get; set; } = [];
    public SubModel Sub { get; set; } = new();
    public List<SubModel> Subs { get; set; } = [];
    public PlainModel Plain { get; set; } = new();
}

/// <summary>
/// Nested model that is generated with signals too
/// </summary>
[GenerateWithSignals]
public class SubModel
{
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Model without the annotation. Inherits the signals from the model that uses it (<see cref="EdgeCases.SignalModel"/>)
/// </summary>
public class PlainModel
{
    public string Name { get; set; } = string.Empty;
}
