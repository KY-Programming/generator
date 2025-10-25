using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Angular.Writers
{
    public class AngularPackageWriter
    {
        public const string BasePackageName = "package";

        public void Write(string name, string fullName, string version, string packagePath, List<AngularPackageDependsOnParameter> dependsOn, string cliVersion, string servicePath, string modelPath, IncrementVersion incrementVersion, bool versionFromNpm)
        {
            if (!InitializeProject(dependsOn, packagePath, name, cliVersion, servicePath, modelPath))
            {
                return;
            }
            if (!UpdateProjectPackageJson(name, fullName, version, dependsOn, packagePath, incrementVersion, versionFromNpm))
            {
                return;
            }
            if (!UpdatePackageJson(packagePath, name))
            {
                return;
            }
        }

        private static bool InitializeProject(List<AngularPackageDependsOnParameter> dependsOn, string packagePath, string name, string cliVersion, string servicePath, string modelPath)
        {
            if (FileSystem.FileExists(packagePath, "../../angular.json"))
            {
                return true;
            }
            cliVersion = cliVersion != null ? "@" + cliVersion.TrimStart('@') : null;
            Logger.Trace("Prepare angular workspace and create a default npm package. This can take a few minutes...");
            StringBuilder builder = new();
            builder.AppendLine($"cd {FileSystem.Combine(packagePath, "../../..")}");
            builder.AppendLine(packagePath.Substring(0, 2));
            builder.AppendLine($"npx -p @angular/cli{cliVersion} ng new {BasePackageName} --create-application false --strict=false");
            builder.AppendLine($"cd {BasePackageName}");
            if (dependsOn.Count > 0)
            {
                IEnumerable<string> dependsOnNames = dependsOn.Select(package => $"{package.Name}@{package.Version}");
                builder.AppendLine($"npm i {string.Join(" ", dependsOnNames)} --save");
            }
            builder.AppendLine($"npm run-script ng g library {name}");
            if (!CommandLineHelper.RunWithTrace(builder))
            {
                Logger.Error("Prepare package failed. See error above...");
                return false;
            }
            string sourcePath = FileSystem.Combine(packagePath, "src");
            FileSystem.DeleteDirectory(sourcePath, "lib");
            modelPath = FileSystem.Combine("./lib", modelPath).Replace(FileSystem.DirectorySeparator(), "/");
            servicePath = FileSystem.Combine("./lib", servicePath).Replace(FileSystem.DirectorySeparator(), "/");
            FileSystem.WriteAllText(FileSystem.Combine(sourcePath, "public-api.ts"), $"export * from '{modelPath}';" + Environment.NewLine + $"export * from '{servicePath}';" + Environment.NewLine);
            return true;
        }

        private static bool UpdateProjectPackageJson(string name, string fullName, string version, List<AngularPackageDependsOnParameter> dependsOn, string packagePath, IncrementVersion incrementVersion, bool versionFromNpm)
        {
            Logger.Trace("Update package.json...");
            string packageJsonPath = FileSystem.Combine(packagePath, "package.json");
            if (!FileSystem.FileExists(packageJsonPath))
            {
                Logger.Error("package.json not found. Try to delete the whole package folder");
                return false;
            }
            JObject packageJson = JsonConvert.DeserializeObject<JObject>(FileSystem.ReadAllText(packageJsonPath));
            JProperty nameProperty = packageJson.Property("name");
            JProperty versionProperty = packageJson.Property("version");
            JObject peerDependencies = packageJson.Property("peerDependencies")?.Value as JObject;
            if (nameProperty == null || versionProperty == null || peerDependencies == null)
            {
                Logger.Error("Invalid package.json structure. Try to delete the whole package folder");
                return false;
            }
            nameProperty.Value = fullName;
            versionProperty.Value = UpdateVersion(fullName, version, versionProperty.Value.ToString(), incrementVersion, versionFromNpm);
            foreach (AngularPackageDependsOnParameter package in dependsOn)
            {
                peerDependencies[package.Name] = package.Version;
            }
            Logger.Trace("Write package.json to disk...");
            FileSystem.WriteAllText(packageJsonPath, JsonConvert.SerializeObject(packageJson, Formatting.Indented));
            return true;
        }

        private static bool UpdatePackageJson(string packagePath, string name)
        {
            Logger.Trace("Add some scripts...");
            string packageJsonPath = FileSystem.Combine(packagePath, "../../package.json");
            if (!FileSystem.FileExists(packageJsonPath))
            {
                Logger.Error("package.json not found. Try to delete the whole package folder");
                return false;
            }
            JObject packageJson = JsonConvert.DeserializeObject<JObject>(FileSystem.ReadAllText(packageJsonPath));
            JObject scriptsProperty = packageJson.Property("scripts")?.Value as JObject;
            if (scriptsProperty == null)
            {
                Logger.Error("Invalid package.json structure. Try to delete the whole package folder");
                return false;
            }
            if (scriptsProperty["build"]?.Value<string>() == "ng build")
            {
                scriptsProperty["build"] = $"ng build {name}";
            }
            scriptsProperty["publish"] ??= $"cd ./dist/{name} && npm publish && cd ../..";
            scriptsProperty["publish:local:linux"] ??= $"cd ./dist/{name} && npm pack && cp *.tgz ../../ && cd ../..";
            scriptsProperty["publish:local:windows"] ??= $"cd ./dist/{name} && npm pack && copy *.tgz ..\\..\\ && cd ../..";
            Logger.Trace("Write package.json to disk...");
            FileSystem.WriteAllText(packageJsonPath, JsonConvert.SerializeObject(packageJson, Formatting.Indented));
            return true;
        }

        private static string UpdateVersion(string fullName, string templateVersionString, string packageJsonVersionString, IncrementVersion incrementVersion, bool versionFromNpm)
        {
            SemanticVersion packageJsonVersion = new(packageJsonVersionString);
            SemanticVersion templateVersion = new(templateVersionString);
            if (versionFromNpm)
            {
                Logger.Trace("Read version from npm...");
                if (!CommandLineHelper.RunWithResult($"npm view {fullName} version", out string npmVersionString))
                {
                    return templateVersionString;
                }
                SemanticVersion npmVersion = new(npmVersionString);
                if (templateVersion.Major > npmVersion.Major
                    || templateVersion.Major == npmVersion.Major && templateVersion.Minor > npmVersion.Minor
                    || templateVersion.Major == npmVersion.Major && templateVersion.Minor == npmVersion.Minor && templateVersion.Build > npmVersion.Build
                )
                {
                    return templateVersionString;
                }
                return IncrementVersionBy(templateVersion, npmVersion, incrementVersion);
            }
            return IncrementVersionBy(templateVersion, packageJsonVersion, incrementVersion);
        }

        private static string IncrementVersionBy(SemanticVersion templateVersion, SemanticVersion currentVersion, IncrementVersion incrementVersion)
        {
            switch (incrementVersion)
            {
                case IncrementVersion.None:
                    return templateVersion.ToString();
                case IncrementVersion.Major:
                    return new SemanticVersion(currentVersion.Major + 1, 0, 0).ToString();
                case IncrementVersion.Minor:
                    return new SemanticVersion(currentVersion.Major, currentVersion.Minor + 1, 0).ToString();
                case IncrementVersion.Patch:
                    return new SemanticVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1).ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(incrementVersion), incrementVersion, null);
            }
        }
    }
}
