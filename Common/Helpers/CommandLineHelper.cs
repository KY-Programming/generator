using System.Diagnostics;
using System.Text;
using KY.Core;

namespace KY.Generator
{
    public static class CommandLineHelper
    {
        public static bool RunWithTrace(StringBuilder commands)
        {
            return RunWithTrace(commands.ToString());
        }

        public static bool RunWithTrace(string commands)
        {
            return Run(commands, out string _);
        }

        public static bool RunWithResult(string commands, out string result)
        {
            return Run(commands, out result, Mode.Output);
        }

        private static bool Run(string commands, out string result, Mode mode = Mode.Trace)
        {
            StringBuilder errorBuilder = new();
            StringBuilder outputBuilder = new();
            Process cmd = new();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/k";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            if (mode == Mode.Trace)
            {
                cmd.OutputDataReceived += (sender, args) => Logger.Trace(args.Data);
            }
            if (mode == Mode.Output)
            {
                commands = "@echo OFF\nset PROMPT=$+\n" + commands;
                cmd.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            }
            cmd.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);
            cmd.Start();
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            cmd.StandardInput.WriteLine(commands);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            result = outputBuilder.ToString().Replace("\r", string.Empty);
            int commandsIndex = result.IndexOf(commands.Replace("\r", string.Empty));
            if (commandsIndex >= 0)
            {
                int commandsEnd = commandsIndex + commands.Length;
                result = result.Substring(commandsEnd, result.Length - commandsEnd).Trim();
            }
            if (errorBuilder.Length > 0 && cmd.ExitCode != 0)
            {
                Logger.Error(errorBuilder.ToString());
                return false;
            }
            if (errorBuilder.Length > 0)
            {
                Logger.Warning(errorBuilder.ToString());
            }
            return true;
        }

        private enum Mode
        {
            Trace,
            Output
        }
    }
}
