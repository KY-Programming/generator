using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KY.Core;
using KY.Generator.Csharp;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Sqlite.Commands;
using KY.Generator.Sqlite.Language;
using KY.Generator.Sqlite.Transfer;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer.Extensions;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Sqlite.Writers
{
    public class SqliteRepositoryWriter : TransferWriter
    {
        private readonly Dictionary<string, string> getMethodMapping = new()
                                                                       {
                                                                           { "System.Boolean", "GetBoolean" },
                                                                           { "System.Byte", "GetByte" },
                                                                           { "System.Char", "GetChar" },
                                                                           { "System.DateTime", "GetDateTime" },
                                                                           { "System.Decimal", "GetDecimal" },
                                                                           { "System.Double", "GetDouble" },
                                                                           { "System.Guid", "GetGuid" },
                                                                           { "System.Int16", "GetInt16" },
                                                                           { "System.Int32", "GetInt32" },
                                                                           { "System.Int64", "GetInt64" },
                                                                           { "System.Single", "GetFloat" },
                                                                           { "System.String", "GetString" },
                                                                           { "System.TimeSpan", "GetTimeSpan" }
                                                                       };

        public SqliteRepositoryWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        public List<FileTemplate> Write(SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<FileTemplate> files = new();
            string repositoryName = parameters.ClassName ?? $"{model.Name}Repository";
            ClassTemplate classTemplate = files.AddFile(parameters.RelativePath, !parameters.SkipHeader, parameters.OutputId)
                                               .AddNamespace(parameters.Namespace)
                                               .AddClass(repositoryName)
                                               .WithUsing("Microsoft.Data.Sqlite");

            FieldTemplate connectionField = classTemplate.AddField("connection", Code.Type("SqliteConnection"));
            classTemplate.AddConstructor()
                         .WithParameter(connectionField.Type, connectionField.Name)
                         .WithCode(Code.This().Field(connectionField).Assign(Code.Local(connectionField.Name)).Close());

            model.Properties.ForEach(property => this.MapType(model.Language, SqliteLanguage.Instance, property.Type));

            this.WriteCreateTable(classTemplate, connectionField, model, parameters);
            this.WriteDropTable(classTemplate, connectionField, model, parameters);
            this.WriteGet(classTemplate, connectionField, model, parameters);
            this.WriteInsert(classTemplate, connectionField, model, parameters);
            this.WriteUpdate(classTemplate, connectionField, model, parameters);
            this.WriteDelete(classTemplate, connectionField, model, parameters);
            return files;
        }

        private void WriteCreateTable(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.Where(p => p.Type.Original != null).ToList();
            if (columns.Count == 0)
            {
                return;
            }
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"CREATE TABLE IF NOT EXISTS {parameters.Table ?? model.Name}")
                      .AppendLine("(");
            SqlitePropertyTransferObject last = columns.Last();
            foreach (SqlitePropertyTransferObject property in columns)
            {
                sqlBuilder.Append($"    {property.Name} {property.Type.Name}");
                if (property.IsNotNull)
                {
                    sqlBuilder.Append(" not null");
                }
                if (property.IsPrimaryKey)
                {
                    sqlBuilder.Append(" primary key");
                }
                if (property.IsAutoIncrement)
                {
                    sqlBuilder.Append(" autoincrement");
                }
                if (property != last)
                {
                    sqlBuilder.Append(",");
                }
                sqlBuilder.AppendLine();
            }

            sqlBuilder.Append(");");
            classTemplate.AddMethod("CreateTable", Code.Void())
                         .WithCode(Code.Using(command))
                         .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sqlBuilder)).Close())
                         .WithCode(Code.Local(command).Method("ExecuteNonQuery").Close());
        }

        private void WriteDropTable(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            string sql = $"DROP TABLE IF EXISTS {parameters.Table ?? model.Name};";
            classTemplate.AddMethod("DropTable", Code.Void())
                         .WithCode(Code.Using(command))
                         .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sql)).Close())
                         .WithCode(Code.Local(command).Method("ExecuteNonQuery").Close());
        }

        private void WriteGet(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.ToList();
            if (columns.Count == 0)
            {
                return;
            }
            classTemplate.AddUsing("System.Collections.Generic");
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append(string.Join(", ", columns.Select(column => column.Name)));
            sqlBuilder.AppendLine();
            sqlBuilder.Append($"FROM {parameters.Table ?? model.Name}");
            GenericTypeTemplate returnType = Code.Generic("List", model.ToTemplate());
            MethodTemplate method = classTemplate.AddMethod("Get", returnType)
                                                 .WithParameter(Code.Type("string"), "filter", Code.Null())
                                                 .WithCode(Code.Using(command))
                                                 .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sqlBuilder)).Close());
            method.WithCode(Code.Declare(returnType, "result", Code.New(returnType)));
            method.WithCode(Code.If(Code.Not().Local("string").Method("IsNullOrEmpty", Code.Local("filter"))).WithCode(
                                Code.Local(command).Field("CommandText").AppendAssign(Code.String("\\nWHERE ").Append(Code.Local("filter"))).Close()
                            ));
            method.WithCode(Code.Using(Code.Declare(Code.Type("SqliteDataReader"), "reader", Code.Local(command).Method("ExecuteReader"))));
            MultilineCodeFragment whileCode = new();
            whileCode.AddLine(Code.Declare(model.ToTemplate(), "entry", Code.New(model.ToTemplate())));
            for (int index = 0; index < columns.Count; index++)
            {
                SqlitePropertyTransferObject column = columns[index];
                if (!this.getMethodMapping.ContainsKey(column.Type.Original.FullName))
                {
                    throw new InvalidOperationException($"Can not write reader method for column '{column.Name}'. No get method for type '{column.Type.Original.FullName}' found. Please contact the support team");
                }
                string getMethod = this.getMethodMapping[column.Type.Original.FullName];
                ICodeFragment getCode;
                if (column.IsNotNull)
                {
                    getCode = Code.Local("reader").Method(getMethod, Code.Number(index));
                }
                else
                {
                    getCode = Code.InlineIf(
                        Code.Local("reader").Method("IsDBNull", Code.Number(index)),
                        Code.Null(),
                        Code.Local("reader").Method(getMethod, Code.Number(index))
                    );
                }
                whileCode.AddLine(Code.Local("entry").Field(column.Name).Assign(getCode).Close());
            }
            whileCode.AddLine(Code.Local("result").Method("Add", Code.Local("entry")).Close());

            method.WithCode(Code.While(Code.Local("reader").Method("Read"), whileCode));
            method.WithCode(Code.Return(Code.Local("result")));
        }

        private void WriteInsert(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.Where(p => !p.IsAutoIncrement && p.Type.Original != null).ToList();
            if (columns.Count == 0)
            {
                return;
            }
            SqlitePropertyTransferObject idColumn = model.Properties.FirstOrDefault(p => p.IsAutoIncrement && p.IsPrimaryKey && p.Type.Original != null);
            bool isAutoIncrement = idColumn != null;
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.Append($"INSERT INTO {parameters.Table ?? model.Name} (");
            sqlBuilder.Append(string.Join(", ", columns.Select(column => column.Name)));
            sqlBuilder.Append(") VALUES (");
            sqlBuilder.Append(string.Join(", ", columns.Select(column => $"@{column.Name.FirstCharToLower()}")));
            sqlBuilder.Append(");");
            if (isAutoIncrement)
            {
                sqlBuilder.Append("\nSELECT last_insert_rowid();");
            }
            MethodTemplate method = classTemplate.AddMethod("Insert", idColumn?.Type.Original.ToTemplate() ?? Code.Void())
                                                 .WithParameter(model.ToTemplate(), "entry")
                                                 .WithCode(Code.Using(command))
                                                 .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sqlBuilder)).Close());
            foreach (SqlitePropertyTransferObject column in columns)
            {
                ICodeFragment valueCode = Code.Local("entry").Field(column.Name);
                if (!column.IsNotNull && (column.Type.Original?.IsNullable ?? column.Type.IsNullable))
                {
                    classTemplate.AddUsing("System");
                    valueCode = Code.NullCoalescing(valueCode, Code.Cast(Code.Type("object")).Local(nameof(DBNull)).Field("Value"));
                }
                method.WithCode(Code.Local(command).Field("Parameters").Method("Add", Code.New(Code.Type("SqliteParameter"),
                                                                                               Code.String($"@{column.Name.FirstCharToLower()}"),
                                                                                               valueCode
                                                                               )).Close());
            }
            if (isAutoIncrement)
            {
                method.WithCode(Code.Return(Code.Cast(method.Type).Cast(Code.Type("long")).Local(command).Method("ExecuteScalar")));
            }
            else
            {
                method.WithCode(Code.Local(command).Method("ExecuteNonQuery").Close());
            }
        }

        private void WriteUpdate(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.Where(p => p.Type.Original != null).ToList();
            if (columns.Count == 0 || columns.All(property => !property.IsPrimaryKey))
            {
                return;
            }
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"UPDATE {parameters.Table ?? model.Name}");
            bool isFirst = true;
            foreach (SqlitePropertyTransferObject column in columns.Where(p => !p.IsPrimaryKey))
            {
                if (isFirst)
                {
                    isFirst = false;
                    sqlBuilder.Append("SET ");
                }
                else
                {
                    sqlBuilder.AppendLine(",")
                              .Append("    ");
                }
                sqlBuilder.Append($"{column.Name} = @{column.Name.FirstCharToLower()}");
            }
            isFirst = true;
            foreach (SqlitePropertyTransferObject column in columns.Where(p => p.IsPrimaryKey))
            {
                if (isFirst)
                {
                    isFirst = false;
                    sqlBuilder.AppendLine()
                              .Append("WHERE ");
                }
                else
                {
                    sqlBuilder.AppendLine(" AND")
                              .Append("    ");
                }
                sqlBuilder.Append($"{column.Name} = @{column.Name.FirstCharToLower()}");
            }
            MethodTemplate method = classTemplate.AddMethod("Update", Code.Void())
                                                 .WithParameter(model.ToTemplate(), "entry")
                                                 .WithCode(Code.Using(command))
                                                 .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sqlBuilder)).Close());
            foreach (SqlitePropertyTransferObject column in columns)
            {
                ICodeFragment valueCode = Code.Local("entry").Field(column.Name);
                if (!column.IsNotNull && column.Type.Original.IsNullable)
                {
                    classTemplate.AddUsing("System");
                    valueCode = Code.NullCoalescing(valueCode, Code.Cast(Code.Type("object")).Local(nameof(DBNull)).Field("Value"));
                }
                method.WithCode(Code.Local(command).Field("Parameters").Method("Add", Code.New(Code.Type("SqliteParameter"),
                                                                                               Code.String($"@{column.Name.FirstCharToLower()}"),
                                                                                               valueCode
                                                                               )).Close());
            }
            method.WithCode(Code.Local(command).Method("ExecuteNonQuery").Close());
        }

        private void WriteDelete(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.Where(p => p.Type.Original != null).ToList();
            if (columns.Count == 0)
            {
                return;
            }
            bool hasPrimaryKey = columns.Any(property => property.IsPrimaryKey);
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"DELETE FROM {parameters.Table ?? model.Name}");
            if (hasPrimaryKey)
            {
                bool isFirst = true;
                foreach (SqlitePropertyTransferObject column in columns.Where(p => p.IsPrimaryKey))
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        sqlBuilder.Append("WHERE ");
                    }
                    else
                    {
                        sqlBuilder.AppendLine(" AND")
                                  .Append("    ");
                    }
                    sqlBuilder.Append($"{column.Name} = @{column.Name.FirstCharToLower()}");
                }
            }
            else
            {
                bool isFirst = true;
                foreach (SqlitePropertyTransferObject column in columns)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        sqlBuilder.Append("WHERE ");
                    }
                    else
                    {
                        sqlBuilder.AppendLine(" AND")
                                  .Append("    ");
                    }
                    sqlBuilder.Append($"{column.Name} = @{column.Name.FirstCharToLower()}");
                }
            }
            MethodTemplate method = classTemplate.AddMethod("Delete", Code.Void())
                                                 .WithParameter(model.ToTemplate(), "entry")
                                                 .WithCode(Code.Using(command))
                                                 .WithCode(Code.Local(command).Field("CommandText").Assign(Code.VerbatimString(sqlBuilder)).Close());
            foreach (SqlitePropertyTransferObject column in columns.Where(property => hasPrimaryKey && property.IsPrimaryKey || !hasPrimaryKey))
            {
                ICodeFragment valueCode = Code.Local("entry").Field(column.Name);
                if (!column.IsNotNull && (column.Type.Original?.IsNullable ?? column.Type.IsNullable))
                {
                    classTemplate.AddUsing("System");
                    valueCode = Code.NullCoalescing(valueCode, Code.Cast(Code.Type("object")).Local(nameof(DBNull)).Field("Value"));
                }
                method.WithCode(Code.Local(command).Field("Parameters").Method("Add", Code.New(Code.Type("SqliteParameter"),
                                                                                               Code.String($"@{column.Name.FirstCharToLower()}"),
                                                                                               valueCode
                                                                               )).Close());
            }
            method.WithCode(Code.Local(command).Method("ExecuteNonQuery").Close());
        }
    }
}
