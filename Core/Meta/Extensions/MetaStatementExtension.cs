using System;

namespace KY.Generator.Meta.Extensions
{
    public static class MetaStatementExtension
    {
        public static MetaStatement Closed(this MetaStatement statement)
        {
            statement.IsClosed = true;
            return statement;
        }
        
        public static MetaStatement WithSeparator(this MetaStatement statement, string separator, Action<IMetaFragmentList> action)
        {
            action(new MetaSeparatorFragmentList(statement.Code, separator));
            return statement;
        }
    }
}