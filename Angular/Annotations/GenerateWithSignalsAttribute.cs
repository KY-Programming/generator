using System;

namespace KY.Generator;

/// <summary>
/// Generates all members of the decorated type (or of all types of the decorated assembly) as Angular signals.
/// The generated Angular service wraps the values on read and unwraps them on write.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateWithSignalsAttribute : Attribute
{ }
