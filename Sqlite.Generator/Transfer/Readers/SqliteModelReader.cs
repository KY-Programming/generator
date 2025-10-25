using System;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer.Readers;

public class SqliteModelReader
{
    private readonly ReflectionModelReader modelReader;

    public SqliteModelReader(ReflectionModelReader modelReader)
    {
        this.modelReader = modelReader;
    }

    public SqliteModelTransferObject Read(Type type)
    {
        ModelTransferObject model = this.modelReader.Read(type);
        SqliteModelTransferObject sqliteModel = new(model);
        return sqliteModel;
    }
}
