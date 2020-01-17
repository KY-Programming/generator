using KY.Generator.Command;
using KY.Generator.Configurations;

namespace KY.Generator.Core.Tests.Models
{
    internal class TestCommand : IGeneratorCommand
    {
        public bool Execute(IConfiguration configuration)
        {
            return true;
        }
    }
}