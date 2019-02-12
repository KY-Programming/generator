namespace KY.Generator.Examples.Reflection.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine("Nothing happens here. Force an rebuild to execute generation!");
            System.Console.WriteLine("Press any key to EXIT...");
            System.Console.ReadKey();
            // You can use cmd instead a own ConsoleApplication
            //Process.Start("KY.Generator.exe", "reflection"
            //                              + " -assembly=KY.Generator.Examples.Reflection.dll"
            //                              + " -name=ExampleType"
            //                              + " -namespace=KY.Generator.Examples.Reflection"
            //                              + " -relativePath=Output"
            //                              + " -language=TypeScript");
            // This happens in Pre-build event
            // The logs are in the package folder .\packages\KY.Generator.CLI.Standalone.x.x.x\tools\Logs
        }
    }
}