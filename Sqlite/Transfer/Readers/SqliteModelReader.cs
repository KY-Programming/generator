using System;
using System.Collections.Generic;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer.Readers
{
    public class SqliteModelReader
    {
        private readonly ReflectionModelReader modelReader;

        public SqliteModelReader(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public SqliteModelTransferObject Read(Type type, List<ITransferObject> transferObjects)
        {
            ModelTransferObject model = this.modelReader.Read(type, transferObjects);
            return new(model);
        }
    }
}
