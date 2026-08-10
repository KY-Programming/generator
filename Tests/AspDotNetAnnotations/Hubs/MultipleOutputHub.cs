using KY.Generator;
using Microsoft.AspNetCore.SignalR;

namespace AspDotNetAnnotations.Hubs;

public interface IMultipleOutputHub
{
    void Test2();
}

/// <summary>
/// The same hub written twice, into two service and two model folders - one annotation per output.
/// </summary>
[GenerateAngularHub("ClientApp/src/app/multiple-output-hub/services-1", "ClientApp/src/app/multiple-output-hub/models-1")]
[GenerateAngularHub("ClientApp/src/app/multiple-output-hub/services-2", "ClientApp/src/app/multiple-output-hub/models-2")]
[GenerateWithRetry(true, 0, 0, 1000, 2000, 5000)]
public class MultipleOutputHub : Hub<IMultipleOutputHub>
{
    public void Test()
    { }
}
