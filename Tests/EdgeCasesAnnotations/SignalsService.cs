using KY.Generator;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
// ReSharper disable UnusedMember.Global

namespace EdgeCasesAnnotations;

/// <summary>
/// Synchronous controller. Covers the wrap/unwrap of models that are generated with
/// <see cref="GenerateWithSignalsAttribute"/>: the service gets a public wrap/unwrap pair per model,
/// wraps every value that is read from the backend into signals and unwraps it again before it is
/// written back. Models without signals stay untouched.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[GenerateAngularService("Output/SignalsService", "Output/SignalsService")]
public class SignalsController : ControllerBase
{
    [HttpGet("[action]")]
    public SignalModel Get()
    {
        return new SignalModel();
    }

    [HttpGet("[action]")]
    public List<SignalModel> GetAll()
    {
        return [];
    }

    [HttpPost("[action]")]
    public string Update(SignalModel model)
    {
        return model.Text;
    }

    [HttpPost("[action]")]
    public void UpdateAll(List<SignalModel> models)
    { }

    [HttpGet("[action]")]
    public PlainModel GetPlain()
    {
        return new PlainModel();
    }

    [HttpPost("[action]")]
    public void UpdatePlain(PlainModel model)
    { }
}

/// <summary>
/// Model with signals. The generated service has to wrap it after every read and unwrap it before every write
/// </summary>
[GenerateWithSignals]
[GeneratePreferInterfaces]
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
[GeneratePreferInterfaces]
public class SubModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime Changed { get; set; }
}

/// <summary>
/// Model without signals. Has to be read and written completely unchanged
/// </summary>
[GeneratePreferInterfaces]
public class PlainModel
{
    public string Name { get; set; } = string.Empty;
}
