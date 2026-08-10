using System;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer.Readers;

public class SqliteModelReader
{
    private readonly Options options;
    private readonly ReflectionModelReader modelReader;

    public SqliteModelReader(Options options, ReflectionModelReader modelReader)
    {
        this.options = options;
        this.modelReader = modelReader;
    }

    public SqliteModelTransferObject Read(Type type)
    {
        ModelTransferObject model = this.modelReader.Read(type);
        SqliteModelTransferObject sqliteModel = new(model);
        // the sqlite model is a new transfer object and does not inherit the options of the model it wraps.
        // Without this mapping it resolves against the root options only and every option set on the read
        // type or its assembly (e.g. GenerateNoHeader) is lost
        this.options.Map(sqliteModel, () => this.options.Get<GeneratorOptions>(type, null));
        return sqliteModel;
    }
}
