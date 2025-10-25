using System.Collections.Generic;
using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer
{
    public class SqliteModelTransferObject : TypeTransferObject
    {
        public ILanguage Language { get; set; }
        public List<SqliteFieldTransferObject> Fields { get; }
        public List<SqlitePropertyTransferObject> Properties { get; }

        public SqliteModelTransferObject(ModelTransferObject model)
            : base(model)
        {
            this.Language = model.Language;
            this.Fields = model.Fields.Select(field => new SqliteFieldTransferObject(field)).ToList();
            this.Properties = model.Properties.Select(property => new SqlitePropertyTransferObject(property)).ToList();
        }
    }
}
