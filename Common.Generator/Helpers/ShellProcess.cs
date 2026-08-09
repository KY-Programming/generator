using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using KY.Core;

namespace KY.Generator
{
    /// <summary>
    /// Runs a command line the way a user would type it in a terminal. The command comes from a parameter, so it can
    /// be anything from a bare executable to a pipe - the platform shell is the only thing that parses all of it.
    /// </summary>
    public static class ShellProcess
    {
        public static int Run(string command, string workingDirectory)
        {
            ProcessStartInfo startInfo = new()
                                         {
                                             WorkingDirectory = workingDirectory,
                                             UseShellExecute = false,
                                             RedirectStandardOutput = true,
                                             RedirectStandardError = true,
                                             CreateNoWindow = true
                                         };
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                startInfo.FileName = "cmd.exe";
                // /s together with the outer quotes keeps cmd from stripping quotes inside the command
                startInfo.Arguments = $"/s /c \"{command}\"";
            }
            else
            {
                startInfo.FileName = "/bin/sh";
                startInfo.Arguments = $"-c \"{command.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
            }
            try
            {
                using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException($"Could not start '{command}'");
                process.OutputDataReceived += (_, args) => Log(args.Data, line => Logger.Trace(line));
                process.ErrorDataReceived += (_, args) => Log(args.Data, line => Logger.Error(line));
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                return process.ExitCode;
            }
            catch (Exception exception)
            {
                Logger.Error($"Could not run '{command}'. {exception.Message}");
                return -1;
            }
        }

        private static void Log(string line, Action<string> write)
        {
            if (!string.IsNullOrEmpty(line))
            {
                write(">> " + line);
            }
        }
    }
}
