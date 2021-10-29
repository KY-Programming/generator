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
            StringBuilder errorBuilder = new();
            Process cmd = new();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/k";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.OutputDataReceived += (sender, args) => Logger.Trace(args.Data);
            cmd.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);
            cmd.Start();
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            cmd.StandardInput.WriteLine(commands);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
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
    }
}
