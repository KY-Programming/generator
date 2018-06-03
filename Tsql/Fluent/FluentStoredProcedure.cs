namespace KY.Generator.Tsql.Fluent
{
    public class FluentStoredProcedure : IFluentLanguageElement
    {
        public string Name { get; }
        public FluentStoredProcedureAction Action { get; }

        public FluentStoredProcedure(string name, FluentStoredProcedureAction action)
        {
            this.Name = name;
            this.Action = action;
        }
    }
}