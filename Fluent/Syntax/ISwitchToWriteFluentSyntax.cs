using System;

namespace KY.Generator.Syntax
{
    public interface ISwitchToWriteFluentSyntax : IFluentSyntax<ISwitchToWriteFluentSyntax>
    {
        /// <summary>
        /// Executes the write actions. Use at least one action method from one of the other generator packages
        /// e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        void Write(Action<IWriteFluentSyntax> action);
    }
}
