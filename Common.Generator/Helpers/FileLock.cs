using System.Diagnostics;
using System.Text;
using KY.Core.DataAccess;
using KY.Generator.Models;

namespace KY.Generator;

/// <summary>
/// A lock that holds across processes, for reading, changing and writing back a file that more than one generator run
/// changes. A solution built with <c>msbuild /m</c> runs the generator of every project in a process of its own, at the
/// same time, so an in-process lock does not see the others.
/// <para>
/// The lock is a file opened with <see cref="FileShare.None"/>. The operating system releases it when the process
/// dies, so a crashed run never leaves a stale lock behind. The lock files are never deleted: on Linux the share mode is
/// an advisory lock on the open file, and deleting it while a second run waits for it would let a third run create and
/// lock a new file of the same name while the second still holds the old one. Instead every path maps onto one of a
/// fixed number of lock files, so the folder never grows - two paths that share one only wait for each other while one
/// of them is written.
/// </para>
/// <para>
/// Never take a lock while holding another one, not even for a different path: it is not reentrant, and two paths can
/// share a lock file. A process that asks for a lock file it already holds waits for itself until the timeout - for
/// two different paths only where they happen to share one, so it fails on some machines and paths and works on all
/// the others
/// </para>
/// </summary>
public static class FileLock
{
    private const int LockCount = 1024;
    private static readonly TimeSpan timeout = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan retryDelay = TimeSpan.FromMilliseconds(10);
    private static readonly Lazy<string> directory = new(() =>
    {
        // FileSystem has no temp folder
        string path = GetDirectory(GeneratorEnvironment.LocalApplicationDataPath, Path.GetTempPath());
        FileSystem.CreateDirectory(path);
        return path;
    });

    /// <summary>
    /// Waits until no other process holds the lock of <paramref name="path"/> and takes it. Dispose the result to
    /// release it
    /// </summary>
    public static IDisposable Acquire(string path)
    {
        string lockPath = FileSystem.Combine(directory.Value, $"{Hash(FileSystem.ToAbsolutePath(path)) % LockCount:x3}.lock");
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                // FileSystem has no way to open a file with a share mode
                return new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException exception)
            {
                if (stopwatch.Elapsed >= timeout)
                {
                    throw new TimeoutException($"Could not lock {path} within {timeout.TotalSeconds:0} seconds. Another process holds {lockPath}", exception);
                }
                Thread.Sleep(retryDelay);
            }
        }
    }

    /// <summary>
    /// Local, not roaming: a lock file has no business being synchronized to another machine. Some environments have no
    /// local application data folder, e.g. a container or a service account without a home directory. The path would
    /// then be relative and resolve into the directory of the generator, which is not writable when it is installed as
    /// a dotnet tool - and every generated file waits for a lock, so not a single one could be written. The temp folder
    /// takes over there: every run of the same environment still decides the same way and finds the same lock files
    /// </summary>
    internal static string GetDirectory(string localApplicationData, string temp)
    {
        return FileSystem.IsAbsolute(localApplicationData)
                   ? FileSystem.Combine(localApplicationData, "locks")
                   : FileSystem.Combine(temp, "KY-Programming", "KY-Generator", "locks");
    }

    /// <summary>
    /// FNV-1a over the lower case path. <see cref="string.GetHashCode()"/> is randomized per process, so it can not
    /// name a file another process has to find. Lower case, because the file systems of Windows and macOS ignore the
    /// case - on Linux two paths that differ only in case merely share a lock
    /// </summary>
    private static uint Hash(string value)
    {
        uint hash = 2166136261;
        foreach (byte current in Encoding.UTF8.GetBytes(value.ToLowerInvariant()))
        {
            hash = (hash ^ current) * 16777619;
        }
        return hash;
    }
}
