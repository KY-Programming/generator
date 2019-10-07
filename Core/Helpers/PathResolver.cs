using System;
using KY.Core.DataAccess;
using KY.Generator.Configuration;

namespace KY.Generator
{
    public static class PathResolver
    {
        public static string Resolve(string relativePath, ConfigurationBase configuration)
        {
            string path = relativePath;
            if (FileSystem.FileExists(path))
            {
                return path;
            }
            path = FileSystem.Combine(Environment.CurrentDirectory, relativePath);
            if (FileSystem.FileExists(path))
            {
                return path;
            }
            path = FileSystem.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (FileSystem.FileExists(path))
            {
                return path;
            }
            path = FileSystem.Combine(FileSystem.Parent(configuration.ConfigurationFilePath), relativePath);
            if (FileSystem.FileExists(path))
            {
                return path;
            }
            throw new InvalidOperationException($"Can not resolve path {relativePath}");
        }
    }
}