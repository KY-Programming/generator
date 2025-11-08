namespace KY.Generator;

public static class WaitHandleExtension
{
    public static async Task<bool> WaitOneAsync(this WaitHandle handle, int millisecondsTimeout = -1, CancellationToken? cancellationToken = null)
    {
        RegisteredWaitHandle? registeredHandle = null;
        CancellationTokenRegistration tokenRegistration = default;
        try
        {
            TaskCompletionSource<bool> tokenSource = new();
            registeredHandle = ThreadPool.RegisterWaitForSingleObject(handle,
                (state, timedOut) => ((TaskCompletionSource<bool>)state).TrySetResult(!timedOut),
                tokenSource,
                millisecondsTimeout,
                true
            );
            tokenRegistration = cancellationToken?.Register(
                state => ((TaskCompletionSource<bool>)state).TrySetCanceled(),
                tokenSource) ?? tokenRegistration;
            return await tokenSource.Task;
        }
        finally
        {
            registeredHandle?.Unregister(null);
            tokenRegistration.Dispose();
        }
    }

    public static Task<bool> WaitOneAsync(this WaitHandle handle, TimeSpan timeout, CancellationToken cancellationToken)
    {
        return handle.WaitOneAsync((int)timeout.TotalMilliseconds, cancellationToken);
    }

    public static Task<bool> WaitOneAsync(this WaitHandle handle, CancellationToken cancellationToken)
    {
        return handle.WaitOneAsync(Timeout.Infinite, cancellationToken);
    }
}
