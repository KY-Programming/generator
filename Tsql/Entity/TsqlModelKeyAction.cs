namespace KY.Generator.Tsql.Entity
{
    public enum TsqlModelKeyActionType
    {
        Add,
        Delete
    }

    public class TsqlModelKeyAction
    {
        public string Name { get; }
        public TsqlModelKeyActionType Type { get; }

        public TsqlModelKeyAction(string name, TsqlModelKeyActionType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}