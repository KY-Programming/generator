using System;

namespace KY.Generator.Angular.Fluent
{
    public interface IAngularWriteSyntax
    {
        /// <summary>
        /// Writes all models (POCOs) from all previous read actions to the output, with some angular specific modifications
        /// </summary>
        IAngularWriteSyntax Models(Action<IAngularModelSyntax> action = null);
        
        /// <summary>
        /// Writes all services from all previous read actions to the output
        /// </summary>
        IAngularWriteSyntax Services(Action<IAngularServiceSyntax> action = null);
    }
}