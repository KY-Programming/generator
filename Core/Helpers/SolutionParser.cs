using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KY.Core.DataAccess;

namespace KY.Generator
{
    public class SolutionParser
    {
        private static readonly Regex projectRegex = new Regex(@"^Project\(""(?<id>{[a-fA-F0-9-]+})""\)\s*=\s*""(?<name>[^""]+)""\s*,\s*""(?<path>[^""]*)""\s*,\s*""(?<localid>{[a-fA-F0-9-]+})""\s*$");

        public VisualStudioSolution Parse(string path)
        {
            if (!FileSystem.FileExists(path))
            {
                return null;
            }
            VisualStudioSolution solution = new VisualStudioSolution();
            string[] lines = FileSystem.ReadAllLines(path);
            foreach (string line in lines)
            {
                Match match = projectRegex.Match(line);
                if (!match.Success)
                {
                    continue;
                }
                solution.Projects.Add(new VisualStudioSolutionProject
                                      {
                                          Id = new Guid(match.Groups["id"].Value),
                                          Name = match.Groups["name"].Value,
                                          Path = match.Groups["path"].Value,
                                          IdInSolution = new Guid(match.Groups["localid"].Value)
                                      }
                );
            }
            return solution;
        }
    }

    public class VisualStudioSolution
    {
        public List<VisualStudioSolutionProject> Projects { get; } = new List<VisualStudioSolutionProject>();
    }

    public class VisualStudioSolutionProject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Guid IdInSolution { get; set; }
    }
}