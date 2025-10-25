﻿using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class BaseWriter : ITemplateWriter
    {

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
        }
        //public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        //{
        //    this.Write(elements.AddClosed().Code, fragment);
        //}

        //public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        //{
        //    fragments.Add("base");
        //}
    }
}