namespace KY.Generator;

/// <summary>
/// Runs a command line after the generation succeeded and every file is written.
/// <para>
/// Mainly for <c>[GenerateInBackground]</c>: that generation outlives the build, so nothing that runs after the
/// build can react to its result - this is the only place left to hook into.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class RunAtSuccessAttribute : Attribute
{
    public string Command { get; }

    public RunAtSuccessAttribute(string command)
    {
        this.Command = command;
    }
}
