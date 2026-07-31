using KY.Generator;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
// ReSharper disable UnusedMember.Global

namespace EdgeCases;

/// <summary>
/// Asynchronous controller. Real ASP.NET controllers are usually async, so this controller covers Task,
/// Task&lt;T&gt;, ActionResult&lt;T&gt; and route/query parameters. The generated Angular service has to unwrap
/// all of those to the same shape as their synchronous counterparts in <see cref="SignalsController"/>.
/// The last actions combine both features: an asynchronous action with a signal model has to unwrap the Task
/// and wrap/unwrap the signals
/// </summary>
[ApiController]
[Route("api/v1/[controller]/[action]")]
[GenerateAngularService("Output/AsyncService", "Output/AsyncService")]
public class AsyncController : ControllerBase
{
    [HttpGet]
    public async Task<AsyncModel> GetAsync()
    {
        await Task.CompletedTask;
        return new AsyncModel();
    }

    [HttpGet]
    public async Task<List<AsyncModel>> GetListAsync()
    {
        await Task.CompletedTask;
        return [];
    }

    [HttpGet("{id}")]
    public async Task<AsyncModel> GetByIdAsync(int id)
    {
        await Task.CompletedTask;
        return new AsyncModel();
    }

    [HttpGet]
    public async Task<ActionResult<AsyncModel>> GetActionResultAsync()
    {
        await Task.CompletedTask;
        return this.Ok(new AsyncModel());
    }

    [HttpGet]
    public Task<string> GetWithoutAsyncKeyword()
    {
        return Task.FromResult(string.Empty);
    }

    [HttpPost]
    public async Task PostAsync(AsyncModel model)
    {
        await Task.CompletedTask;
    }

    [HttpPost]
    public async Task<string> PostWithQueryAsync([FromQuery] string filter, AsyncModel model)
    {
        await Task.CompletedTask;
        return filter;
    }

    [HttpGet]
    public AsyncModel GetSynchronous()
    {
        return new AsyncModel();
    }

    [HttpGet]
    public async Task<SignalModel> GetSignalAsync()
    {
        await Task.CompletedTask;
        return new SignalModel();
    }

    [HttpGet]
    public async Task<List<SignalModel>> GetSignalListAsync()
    {
        await Task.CompletedTask;
        return [];
    }

    [HttpPost]
    public async Task UpdateSignalAsync(SignalModel model)
    {
        await Task.CompletedTask;
    }
}

/// <summary>
/// Model without signals that is used by the asynchronous controller
/// </summary>
[GeneratePreferInterfaces]
public class AsyncModel
{
    public string Name { get; set; } = "";
    public int Value { get; set; }
    public string? NullableName { get; set; }
}
