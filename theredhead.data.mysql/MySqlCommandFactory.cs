using MySql.Data.MySqlClient;
using theredhead.text;
using theredhead.data.sql;

namespace theredhead.data.mysql;

public class MySqlCommandFactory : BaseSqlCommandFactory<MySqlConnection>
{
    protected override QuoteKind objectNameQuoteKind => QuoteKind.Backtick;
    protected override QuoteKind parameterNameQuoteKind => new QuoteKind("@", "");

    public MySqlCommandFactory(MySqlConnection connection) : base(connection)
    {
    }
}

public static class MySqlConnectionExtensions {
    public static MySqlCommandFactory GetCommandFactory(this MySqlConnection connection) {
        return new MySqlCommandFactory(connection);
    }
}