namespace KY.Generator.Examples.Reflection.Attributes
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine("Nothing happens here. Force an rebuild to execute generation!");
            System.Console.WriteLine("Press any key to EXIT...");
            System.Console.ReadKey();
            
            // The magic happens in Post-build event
            // The logs are in the package folder .\packages\KY.Generator.CLI.x.x.x\tools\Logs
        }
    }
}