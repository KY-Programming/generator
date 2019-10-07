namespace KY.Generator.Templates
{
    public class PropertyValueTemplate
    {
        public string Name { get; }
        public ICodeFragment Value { get; }

        public PropertyValueTemplate(string name, ICodeFragment value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}