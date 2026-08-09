namespace KY.Generator;

public interface ITsqlFromDatabaseOrReadSyntax : ITsqlFromDatabaseSyntax, ITsqlReadSyntax
{
    /// <summary>
    /// The schema <see cref="UseTable(string)"/> and <see cref="UseAll"/> read from. Without it every schema of the
    /// database is read - T-SQL defaults to "dbo", but a table is not required to live there.
    /// </summary>
    ITsqlFromDatabaseOrReadSyntax UseSchema(string schema);

    /// <summary>Reads a single table from the schema set with <see cref="UseSchema"/>.</summary>
    ITsqlFromDatabaseOrReadSyntax UseTable(string table);

    /// <summary>Reads a single table from an explicit schema, independent of <see cref="UseSchema"/>.</summary>
    ITsqlFromDatabaseOrReadSyntax UseTable(string schema, string table);

    /// <summary>Reads every table - of the schema set with <see cref="UseSchema"/>, or of the whole database.</summary>
    ITsqlFromDatabaseOrReadSyntax UseAll();

    /// <summary>Namespace of the generated models.</summary>
    ITsqlFromDatabaseOrReadSyntax UseNamespace(string @namespace);
}
