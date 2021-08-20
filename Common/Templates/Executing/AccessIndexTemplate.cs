﻿namespace KY.Generator.Templates
{
    public class AccessIndexTemplate : ChainedCodeFragment
    {
        public ICodeFragment Code { get; }
        public override string Separator => string.Empty;

        public AccessIndexTemplate(ICodeFragment code)
        {
            this.Code = code;
        }
    }
}