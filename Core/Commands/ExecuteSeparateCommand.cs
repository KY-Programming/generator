using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Transfer;

namespace KY.Generator.Commands
{
    internal class ExecuteSeparateCommand : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            ExecuteConfiguration configuration = (ExecuteConfiguration)configurationBase;
            if (configuration.File == configuration.Environment.ConfigurationFilePath)
            {
                throw new InvalidOperationException($"{configuration.File} cyclic execution detected. Generation aborted!");
            }
            FileSystemInstance fileSystem = new FileSystemInstance(FileSystem.Parent(configuration.Environment.ConfigurationFilePath));
            Logger.Trace($"Execute {configuration.File} in new process...");
            string executable = Assembly.GetEntryAssembly().Location;
            string configurationPath = fileSystem.ToAbsolutePath(configuration.File);
            Process process = new Process();
            if (executable.EndsWith(".exe"))
            {
                process.StartInfo.FileName = executable;
                process.StartInfo.Arguments = $"execute -file=\"{configurationPath}\" -forwardLogging";
            }
            else
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"\"{executable}\" execute -file=\"{configurationPath}\" -forwardLogging";
            }
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WorkingDirectory = fileSystem.Root;
            process.Start();
            Logger.Trace("Redirected output from process:");
            string line;
            while ((line = process.StandardOutput.ReadLine()) != null) 
            {
                Logger.Trace(line);
            }
            Logger.Trace($"Process exited with code {process.ExitCode}");
            return process.ExitCode == 0;
        }
    }
}