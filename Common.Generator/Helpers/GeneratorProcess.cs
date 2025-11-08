using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;

namespace KY.Generator
{
    public static class GeneratorProcess
    {
        public static Process Start(params IGeneratorCommand[] commands)
        {
            return Start(commands.ToList());
        }

        public static Process Start(IEnumerable<IGeneratorCommand> commands, string arguments = null)
        {
            return Start(null, commands, arguments);
        }

        public static Process Start(string location, IEnumerable<IGeneratorCommand> commands, string arguments = null)
        {
            return Start(location, commands, arguments, false);
        }

        public static Process StartHidden(params IGeneratorCommand[] commands)
        {
            return StartHidden(commands.ToList());
        }

        public static Process StartHidden(IEnumerable<IGeneratorCommand> commands, string arguments = null)
        {
            return StartHidden(null, commands, arguments);
        }

        public static Process StartHidden(string location, IEnumerable<IGeneratorCommand> commands, string arguments = null)
        {
            return Start(location, commands, arguments, true);
        }

        private static Process Start(string location, IEnumerable<IGeneratorCommand> commands, string arguments, bool hidden)
        {
            string allArguments = string.Join(" ", commands);
            if (arguments != null)
            {
                allArguments += arguments;
            }
            return Start(location, allArguments, hidden);
        }

        private static Process Start(string location, string arguments, bool hidden)
        {
            location ??= Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException("No location found");
            string locationExe = location.Replace(".dll", ".exe");
            ProcessStartInfo startInfo = new();
            if (hidden)
            {
                startInfo.UseShellExecute = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && FileSystem.FileExists(locationExe))
            {
                // Always use the .exe on Windows to fix the dotnet.exe x86 problem
                startInfo.FileName = locationExe;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && location.EndsWith(".exe"))
            {
                startInfo.FileName = location;
            }
            else
            {
                startInfo.FileName = "dotnet";
                startInfo.Arguments = location + " ";
            }
            if (arguments != null)
            {
                startInfo.Arguments += arguments;
            }
            // Logger.Trace($"Start process: {startInfo.FileName} {startInfo.Arguments}");
            return Process.Start(startInfo);
        }
    }
}
