﻿using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Json.Commands;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public class JsonReadSyntax : IJsonReadSyntax, IExecutableSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;

        public List<IGeneratorCommand> Commands { get; } = new();

        public JsonReadSyntax(IReadFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IJsonReadSyntax FromFile(string relativePath)
        {
            JsonReadCommand command = this.syntax.Resolver.Create<JsonReadCommand>();
            command.Parameters.RelativePath = relativePath;
            this.Commands.Add(command);
            return this;
        }
    }
}
