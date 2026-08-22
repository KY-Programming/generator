namespace KY.Generator;

/// <summary>
/// Runs a command line after the generation failed - the counterpart of <see cref="RunAtSuccessAttribute"/>.
/// <para>
/// Mainly for <c>[GenerateInBackground]</c>: that generation outlives the build, so a failure has nowhere to
/// surface - the build is long green and the process is hidden. This is the only place left to react to it.
/// </para>
/// <para>
/// The commands are read off the loaded assemblies, so a run that fails before the assembly is loaded - a bad
/// argument, an assembly that can not be read, a missing license - has none of them and runs nothing. Whoever
/// waits for the result needs a timeout regardless.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class RunAtFailureAttribute : Attribute
{
    public string Command { get; }

    public RunAtFailureAttribute(string command)
    {
        this.Command = command;
    }
}
