﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KY.Generator
{
    internal class Program
    {
        private static string SharedPath { get; } = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), "netstandard2.0");

        private static void Main(string[] args)
        {
            bool success = LoadShared("KY.Core.Common")
                           && LoadShared("KY.Generator.Common")
                           && Run(args);
            if (!success)
            {
                Environment.ExitCode = 1;
            }
        }

        private static bool LoadShared(string assemblyName)
        {
            string coreFileName = Path.Combine(SharedPath, assemblyName + ".dll");
            if (!File.Exists(coreFileName))
            {
                Console.WriteLine($"Error: {assemblyName} not found in {SharedPath}");
                return false;
            }
            Assembly.LoadFrom(coreFileName);
            return true;
        }

        private static bool Run(string[] args)
        {
            Assembly core = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.FullName.StartsWith("KY.Generator.Common,"));
            Type type = core.GetType("KY.Generator.Main");
            if (type == null)
            {
                Console.WriteLine("Error: KY.Generator.Main not found");
                return false;
            }
            MethodInfo runMethod = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
            if (runMethod == null)
            {
                Console.WriteLine("Error: KY.Generator.Main.Run not found");
                return false;
            }
            object[] parameter = new object[1];
            parameter[0] = args;
            return (bool)runMethod.Invoke(null, parameter);
        }
    }
}
