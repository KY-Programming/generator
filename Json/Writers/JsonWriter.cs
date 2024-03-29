﻿using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Json.Writers
{
    internal class JsonWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public JsonWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void FormatNames()
        {
            this.resolver.Create<ObjectWriter>().FormatNames();
        }

        public void Write(string relativePath, bool withReader)
        {
            this.resolver.Create<ObjectWriter>().Write(relativePath, withReader);
        }
    }
}
