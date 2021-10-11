using System;

namespace KY.Generator.Syntax
{
    public interface ISwitchToWriteFluentSyntax : IFluentSyntax<ISwitchToWriteFluentSyntax>
    {
        /// <summary>
        /// Executes the write actions. Use at least one action method from one of the other generator packages
        /// e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        /// <footer>see <a href="https://generator.ky-programming.de/core/documentation/fluent-api#Write">`.Write()` on generator.ky-programming.de</a></footer>
        void Write(Action<IWriteFluentSyntax> action);
    }
}
