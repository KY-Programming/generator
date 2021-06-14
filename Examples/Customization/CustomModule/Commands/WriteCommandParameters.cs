using KY.Generator.Command;

namespace CustomModule.Commands
{
    // 5. Create parameters for command
    //      The parameters holds all the settings made by the user
    //  a. Derive from GeneratorCommandParameters
    //  b. Add your settings (like here Message property)
    internal class WriteCommandParameters : GeneratorCommandParameters
    {
        public string Message { get; set; }
        public string Class { get; set; }
        public string Namespace { get; set; }
    }
}