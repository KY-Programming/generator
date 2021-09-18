using System;

namespace KY.Generator.Syntax
{
    public interface ISwitchToReadFluentSyntax : IFluentSyntax<ISwitchToReadFluentSyntax>
    {
        /// <summary>
        /// Executes the read actions. Use at least one action method from one of the other generator packages
        /// e.g. <code>KY.Generator.AspDotNet</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action);
    }
}
