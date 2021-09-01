using System;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer.Readers
{
    public class SqliteModelReader
    {
        private readonly ReflectionModelReader modelReader;
        private readonly Options options;

        public SqliteModelReader(ReflectionModelReader modelReader, Options options)
        {
            this.modelReader = modelReader;
            this.options = options;
        }

        public SqliteModelTransferObject Read(Type type)
        {
            ModelTransferObject model = this.modelReader.Read(type);
            IOptions modelOptions = this.options.Get(model);
            SqliteModelTransferObject sqliteModel = new(model);
            this.options.Set(sqliteModel, modelOptions);
            return sqliteModel;
        }
    }
}
