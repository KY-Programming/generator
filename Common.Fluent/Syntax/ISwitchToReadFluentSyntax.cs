using System;

namespace KY.Generator.Syntax
{
    public interface ISwitchToReadFluentSyntax : IFluentSyntax<ISwitchToReadFluentSyntax>
    {
        /// <summary>
        /// Executes the read actions. Use at least one action method from one of the other generator packages
        /// e.g. <code>KY.Generator.AspDotNet</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        /// <footer>see <a href="https://generator.ky-programming.de/core/documentation/fluent-api#Read">`.Read()` on generator.ky-programming.de</a></footer>
        ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action);
    }
}
