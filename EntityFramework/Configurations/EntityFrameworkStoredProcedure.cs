namespace KY.Generator.EntityFramework.Configurations
{
    public class EntityFrameworkStoredProcedure
    {
        public string Name { get; }

        public EntityFrameworkStoredProcedure(string name)
        {
            this.Name = name;
        }
    }
}