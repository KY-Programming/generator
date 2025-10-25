using KY.Generator.Command;

namespace KY.Generator.Angular.Commands;

public class AngularServiceCommandParameters : GeneratorCommandParameters
{
    public string? Name { get; set; }
    public string? RelativeModelPath { get; set; }
    public bool EndlessTries { get; set; }
    public List<int>? Timeouts { get; set; }
    public string? HttpClient { get; set; }
    public string? HttpClientUsing { get; set; }
    public string? HttpClientGet { get; set; }
    public AngularHttpClientMethodOptions HttpClientGetOptions { get; set; } = new();
    public string? HttpClientPost { get; set; }
    public AngularHttpClientMethodOptions HttpClientPostOptions { get; set; } = new();
    public string? HttpClientPatch { get; set; }
    public AngularHttpClientMethodOptions HttpClientPatchOptions { get; set; } = new();
    public string? HttpClientPut { get; set; }
    public AngularHttpClientMethodOptions HttpClientPutOptions { get; set; } = new();
    public string? HttpClientDelete { get; set; }
    public AngularHttpClientMethodOptions HttpClientDeleteOptions { get; set; } = new();

    public static string[] Names { get; } = [..ToCommand(nameof(AngularServiceCommand))];

    public AngularServiceCommandParameters()
        : base(Names.First())
    { }
}
