namespace KY.Generator.Tsql.Entity
{
    public class TsqlStoredProcedure
    {
        public string Name { get; }

        public TsqlStoredProcedure(string name)
        {
            this.Name = name;
        }
    }
}