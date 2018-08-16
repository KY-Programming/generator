using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.AspDotNet.Templates
{
    internal interface ITemplate
    {
        string Name { get; }
        IEnumerable<UsingTemplate> Usings { get; }
        bool UseOwnCache { get; }
        bool ValidateInput { get; }
    }
}