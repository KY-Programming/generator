﻿using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Output;

namespace KY.Generator.Core.Tests.Models
{
    public class TestOutput : IOutput
    {
        public List<TestFile> Files { get; } = new();

        public void Write(string fileName, string content, Guid? outputId)
        {
            this.Files.Add(new TestFile(fileName, content));
        }

        public void Delete(string fileName)
        {
            this.Files.Remove(this.Files.FirstOrDefault(x => x.Name == fileName));
        }

        public void DeleteAllRelatedFiles(Guid? outputId, string relativePath = null)
        { }

        public void Execute()
        { }

        public void Move(string relativePath)
        { }
    }

    public class TestFile
    {
        public string Name { get; }
        public string Content { get; }

        public TestFile(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}