﻿using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CastWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            CastTemplate template = (CastTemplate)fragment;
            output.Add("(")
                  .Add(template.Type)
                  .Add(")");
        }
    }
}
