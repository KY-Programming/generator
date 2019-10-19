namespace KY.Generator.Tsql.Type
{
    public class TsqlColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public int Length { get; set; }
        public bool IsNullable { get; set; }
        public string ForeignKeyName { get; set; }
        public string ForeignKeyType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey => !string.IsNullOrEmpty(this.ForeignKeyName);
        public bool IsIdentity { get; set; }
        public bool IsUnicode { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public string DefaultValue { get; set; }

        public bool IsFixedString => this.Type == "char" || this.Type == "nchar";
        public bool IsString => this.IsFixedString || this.Type == "varchar" || this.Type == "nvarchar";
        public bool IsDecimal => this.Type == "decimal";
        public bool IsInt => this.Type == "smallint" || this.Type == "int" || this.Type == "bigint";
        public bool HasDefault => this.DefaultValue != null;
    }
}