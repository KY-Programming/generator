using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KY.Core;
using KY.Generator.Tsql.Properties;

namespace KY.Generator.Tsql.Type
{
    public class TsqlTypeReader : IDisposable
    {
        private readonly SqlConnection connection;
        private readonly Dictionary<string, List<TsqlColumn>> columnsCache;
        private readonly bool throwErrorOnEntityGeneration;

        public Exception LastError { get; private set; }

        public TsqlTypeReader(string connectionString, bool throwErrorOnEntityGeneration = true)
        {
            this.throwErrorOnEntityGeneration = throwErrorOnEntityGeneration;
            this.connection = new SqlConnection(connectionString);
            this.columnsCache = new Dictionary<string, List<TsqlColumn>>();
        }

        public List<TsqlColumn> GetColumns(string schema, string table)
        {
            this.LastError = null;
            string key = $"{schema}.{table}";
            if (this.columnsCache.ContainsKey(key))
            {
                return this.columnsCache[key];
            }
            List<TsqlColumn> columns = new List<TsqlColumn>();
            try
            {
                this.OpenConnection();
                using (SqlCommand command = this.connection.CreateCommand())
                {
                    command.CommandText = Resources.ReadColumnsCommand;
                    command.Parameters.AddWithValue("schema", schema);
                    command.Parameters.AddWithValue("name", table);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                columns.Add(this.ReadColumn(reader));
                            }
                        }
                        this.columnsCache.Add(key, columns);
                        if (columns.All(x => !x.IsPrimaryKey))
                        {
                            columns.ForEach(x => x.IsPrimaryKey = !x.IsNullable);
                        }
                    }
                }
            }
            catch (SqlException exception)
            {
                Logger.Error($"Can not read columns from {schema}.{table}", nameof(this.GetColumns), nameof(TsqlTypeReader));
                Logger.Error(exception);
                this.LastError = exception;
                if (this.throwErrorOnEntityGeneration)
                {
                    throw;
                }
            }
            return columns;
        }

        private TsqlColumn ReadColumn(SqlDataReader reader)
        {
            TsqlColumn column = new TsqlColumn();
            column.Name = reader.GetString(reader.GetOrdinal("COLUMN_NAME"));
            column.Type = reader.GetString(reader.GetOrdinal("DATA_TYPE"));
            column.Order = reader.GetInt32(reader.GetOrdinal("ORDINAL_POSITION"));
            column.IsNullable = reader.GetString(reader.GetOrdinal("IS_NULLABLE")) == "YES";
            column.Length = reader.GetInt32(reader.GetOrdinal("CHARACTER_MAXIMUM_LENGTH"), 0);
            column.IsPrimaryKey = reader.GetInt32(reader.GetOrdinal("IS_PRIMARY")) > 0;
            column.ForeignKeyName = reader.GetString(reader.GetOrdinal("FOREIGN_NAME"), null)?.Split('_').Last();
            column.ForeignKeyType = reader.GetString(reader.GetOrdinal("FOREIGN_TABLE_NAME"), null);
            column.IsIdentity = reader.GetInt32(reader.GetOrdinal("IS_IDENTITY")) > 0;
            column.IsUnicode = reader.GetInt32(reader.GetOrdinal("IS_UNICODE")) > 0;
            column.Precision = reader.GetByte(reader.GetOrdinal("NUMERIC_PRECISION"), 0);
            column.Scale = reader.GetInt32(reader.GetOrdinal("NUMERIC_SCALE"), 0);
            column.DefaultValue = reader.GetString(reader.GetOrdinal("COLUMN_DEFAULT"), null)?.Trim('(', ')', '\'');
            return column;
        }

        public List<TsqlColumn> GetPrimaryKeys(string schema, string table)
        {
            return this.GetColumns(schema, table).Where(x => x.IsPrimaryKey).ToList();
        }

        public List<TsqlColumn> GetForeignKeys(string schema, string table)
        {
            return this.GetColumns(schema, table).Where(x => x.IsForeignKey).ToList();
        }

        public List<TsqlNavigationProperty> GetNavigationProperties(string schema, string table)
        {
            Dictionary<string, TsqlNavigationProperty> properties = new Dictionary<string, TsqlNavigationProperty>();
            List<TsqlColumn> foreignKeys = this.GetForeignKeys(schema, table);
            foreach (TsqlColumn foreignKey in foreignKeys)
            {
                if (!properties.ContainsKey(foreignKey.ForeignKeyName))
                {
                    properties.Add(foreignKey.ForeignKeyName, new TsqlNavigationProperty(foreignKey.ForeignKeyName, foreignKey.ForeignKeyType));
                }
            }
            return properties.Values.ToList();
        }

        public List<List<string>> GetValues(string schema, string table, params string[] columns)
        {
            List<List<string>> rows = new List<List<string>>();
            this.OpenConnection();
            SqlCommand command = this.connection.CreateCommand();
            command.CommandText = string.Format(Resources.ReadValuesCommand, string.Join(", ", columns), schema, table);
            command.Parameters.AddWithValue("schema", schema);
            command.Parameters.AddWithValue("name", table);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int ordinal = 0; ordinal < columns.Length; ordinal++)
                    {
                        row.Add(reader.GetValue(ordinal).ToString());
                    }
                    rows.Add(row);
                }
            }
            return rows;
        }

        public List<TsqlParameter> GetParameters(string schema, string storedProcedure)
        {
            this.LastError = null;
            List<TsqlParameter> list = new List<TsqlParameter>();
            try
            {
                this.OpenConnection();
                SqlCommand command = this.connection.CreateCommand();
                command.CommandText = Resources.ReadParametersCommand;
                command.Parameters.AddWithValue("schema", schema);
                command.Parameters.AddWithValue("name", storedProcedure);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TsqlParameter parameter = new TsqlParameter();
                            parameter.Name = reader.GetString(reader.GetOrdinal("PARAMETER_NAME"));
                            parameter.Type = reader.GetString(reader.GetOrdinal("DATA_TYPE"));
                            parameter.Order = reader.GetInt32(reader.GetOrdinal("ORDINAL_POSITION"));
                            parameter.IsNullable = true;
                            list.Add(parameter);
                        }
                    }
                }
            }
            catch (SqlException exception)
            {
                Logger.Error($"Can not read parameters from {schema}.{storedProcedure}", nameof(this.GetParameters), nameof(TsqlTypeReader));
                Logger.Error(exception);
                this.LastError = exception;
                if (this.throwErrorOnEntityGeneration)
                {
                    throw;
                }
            }
            return list;
        }

        public List<TsqlColumn> GetColumnsFromStoredProcedure(string schema, string storedProcedure)
        {
            this.LastError = null;
            string key = $"{schema}.{storedProcedure}";
            if (this.columnsCache.ContainsKey(key))
                return this.columnsCache[key];

            List<TsqlColumn> columns = new List<TsqlColumn>();
            try
            {
                this.OpenConnection();
                SqlCommand command = this.connection.CreateCommand();
                command.CommandText = string.Format(Resources.ReadResultCommand, key);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        columns.Add(this.ReadColumn(reader));
                    }
                }
                this.columnsCache.Add(key, columns);
                if (columns.All(x => !x.IsPrimaryKey))
                {
                    columns.ForEach(x => x.IsPrimaryKey = !x.IsNullable);
                }
            }
            catch (SqlException exception)
            {
                Logger.Error($"Can not read columns from {schema}.{storedProcedure}", nameof(this.GetColumnsFromStoredProcedure), nameof(TsqlTypeReader));
                Logger.Error(exception);
                this.LastError = exception;
                if (this.throwErrorOnEntityGeneration)
                {
                    throw;
                }
            }
            return columns;
        }

        private void OpenConnection()
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
        }

        public void Dispose()
        {
            this.connection?.Dispose();
        }
    }
}