using KY.Generator;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs;

public interface IMultipleOutputHub
{
    void Test2();
}

[GenerateAngularHub("\\ClientApp\\src\\app\\multiple\\services-1", "\\ClientApp\\src\\app\\multiple\\models-1")]
[GenerateAngularHub("\\ClientApp\\src\\app\\multiple\\services-2", "\\ClientApp\\src\\app\\multiple\\models-2")]
[GenerateWithRetry(true, 0, 0, 1000, 2000, 5000)]
public class MultipleOutputHub : Hub<IMultipleOutputHub>
{
    public void Test()
    {
        
    }
}
