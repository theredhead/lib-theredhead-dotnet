using theredhead.text;
using theredhead.data.sql;
using Microsoft.Data.Sqlite;

namespace theredhead.data.sqlite;

public class SqliteCommandFactory : BaseSqlCommandFactory<SqliteConnection>
{
    protected override QuoteKind objectNameQuoteKind => QuoteKind.Double;
    protected override QuoteKind parameterNameQuoteKind => new QuoteKind("@", "");

    public SqliteCommandFactory(SqliteConnection connection) : base(connection)
    {
    }
}

public static class SqliteConnectionExtensions {
    public static SqliteCommandFactory GetCommandFactory(this SqliteConnection connection) {
        return new SqliteCommandFactory(connection);
    }
}