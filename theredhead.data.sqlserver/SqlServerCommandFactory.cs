using theredhead.text;
using theredhead.data.sql;

using Microsoft.Data.SqlClient;

namespace theredhead.data.sqlserver;

public class SqlServerCommandFactory : BaseSqlCommandFactory<SqlConnection>
{
    protected override QuoteKind objectNameQuoteKind => QuoteKind.Block;
    protected override QuoteKind parameterNameQuoteKind => new QuoteKind("@", "");

    public SqlServerCommandFactory(SqlConnection connection) : base(connection)
    {
    }
}

public static class SqlServerConnectionExtensions {
    public static SqlServerCommandFactory GetCommandFactory(this SqlConnection connection) {
        return new SqlServerCommandFactory(connection);
    }
}