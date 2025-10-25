using System;
using System.Runtime.InteropServices;
using System.Text;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Angular.Writers
{
    public class AngularPackageBuilder
    {
        public void Build(string packagePath)
        {
            Logger.Trace("Build npm package. This can take a few minutes...");
            StringBuilder builder = new();
            builder.AppendLine($"cd {FileSystem.Combine(packagePath, "../..")}");
            builder.AppendLine(packagePath.Substring(0, 2));
            builder.AppendLine($"npm run build");
            if (!CommandLineHelper.RunWithTrace(builder))
            {
                Logger.Error("Package build failed. See error above...");
            }
        }

        public void Publish(string packagePath)
        {
            Logger.Trace("Publish npm package. This can take a few minutes...");
            StringBuilder builder = new();
            builder.AppendLine($"cd {FileSystem.Combine(packagePath, "../..")}");
            builder.AppendLine(packagePath.Substring(0, 2));
            builder.AppendLine($"npm run publish");
            if (!CommandLineHelper.RunWithTrace(builder))
            {
                Logger.Error("Package publish failed. See error above...");
            }
        }

        public void PublishLocal(string packagePath)
        {
            Logger.Trace("Publish npm package local. This can take a few minutes...");
            StringBuilder builder = new();
            builder.AppendLine($"cd {FileSystem.Combine(packagePath, "../..")}");
            builder.AppendLine(packagePath.Substring(0, 2));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                builder.AppendLine($"npm run publish:local:linux");
            }
            else
            {
                builder.AppendLine($"npm run publish:local:windows");
            }
            if (!CommandLineHelper.RunWithTrace(builder))
            {
                Logger.Error("Package local publish failed. See error above...");
            }
        }
    }
}
