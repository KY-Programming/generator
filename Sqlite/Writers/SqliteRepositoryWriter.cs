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

            this.WriteCreateTable(classTemplate, connectionField, model, parameters);
            this.WriteDropTable(classTemplate, connectionField, model, parameters);
            this.WriteInsert(classTemplate, connectionField, model, parameters);
            this.WriteUpdate(classTemplate, connectionField, model, parameters);
            this.WriteDelete(classTemplate, connectionField, model, parameters);
            return files;
        }

        private void WriteCreateTable(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"CREATE TABLE IF NOT EXISTS {parameters.Table ?? model.Name}")
                      .AppendLine("(");
            SqlitePropertyTransferObject last = model.Properties.Last();
            foreach (SqlitePropertyTransferObject property in model.Properties)
            {
                this.MapType(model.Language, SqliteLanguage.Instance, property.Type);
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

        private void WriteInsert(ClassTemplate classTemplate, FieldTemplate connectionField, SqliteModelTransferObject model, SqliteWriteRepositoryCommandParameters parameters)
        {
            List<SqlitePropertyTransferObject> columns = model.Properties.Where(p => !p.IsAutoIncrement).ToList();
            SqlitePropertyTransferObject idColumn = model.Properties.FirstOrDefault(column => column.IsAutoIncrement && column.IsPrimaryKey);
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
            if (model.Properties.All(property => !property.IsPrimaryKey))
            {
                return;
            }
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"UPDATE {parameters.Table ?? model.Name}");
            bool isFirst = true;
            foreach (SqlitePropertyTransferObject column in model.Properties.Where(p => !p.IsPrimaryKey))
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
            foreach (SqlitePropertyTransferObject column in model.Properties.Where(p => p.IsPrimaryKey))
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
            foreach (SqlitePropertyTransferObject column in model.Properties)
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
            bool hasPrimaryKey = model.Properties.Any(property => property.IsPrimaryKey);
            DeclareTemplate command = Code.Declare(Code.Type("SqliteCommand"), "command", Code.This().Field(connectionField).Method("CreateCommand"));
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine()
                      .AppendLine($"DELETE FROM {parameters.Table ?? model.Name}");
            if (hasPrimaryKey)
            {
                bool isFirst = true;
                foreach (SqlitePropertyTransferObject column in model.Properties.Where(p => p.IsPrimaryKey))
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
                foreach (SqlitePropertyTransferObject column in model.Properties)
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
            foreach (SqlitePropertyTransferObject column in model.Properties.Where(property => hasPrimaryKey && property.IsPrimaryKey || !hasPrimaryKey))
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
    }
}
