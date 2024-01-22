using System;
using System.Data;
using theredhead.text;

namespace theredhead.data.sql;

public interface ISqlCommandFactory
{
    IDbCommand CreateInsertCommand(string tableName, Dictionary<string, object> columnsAndValues);
    IDbCommand CreateUpdateCommand(string tableName, Dictionary<string, object> key, Dictionary<string, object> columnsAndValues);
    IDbCommand CreateDeleteCommand(string tableName, Dictionary<string, object> key);
    IDbCommand CreateSelectCommand(string tableName, Dictionary<string, object> key, IEnumerable<string>? columns = null);
}

public interface ISqlCommandFactory<T> : ISqlCommandFactory where T: IDbConnection
{
}

public class BaseSqlCommandFactory<T>(T connection) : ISqlCommandFactory<T> where T : IDbConnection
{
    private readonly QuoteKind _parameterNameQuoteKind = new ("@", "");
    protected virtual QuoteKind objectNameQuoteKind => QuoteKind.Double;
    protected virtual QuoteKind parameterNameQuoteKind => _parameterNameQuoteKind;

    protected T Connection => connection;

    private string QuoteObjectName(string objectName)
    {
        return objectNameQuoteKind.Quote(objectName);
    }
    private string ParameterName(string objectName)
    {
        return parameterNameQuoteKind.Quote(objectName);
    }
    private string ExpandWhereText(Dictionary<string, object> key)
    {
        if (key == null || key.Count == 0) return "";

        return "WHERE " + key.Keys.Select(c => $"{QuoteObjectName(c)} = {ParameterName(c)}").JoinedBy(" AND ");
    }

    public IDbCommand CreateDeleteCommand(string tableName, Dictionary<string, object> key)
    {
        var arguments = new CommandArguments();
        var quotedTableName = QuoteObjectName(tableName);
        var where = ExpandWhereText(key);
        var commandText = $"DELETE FROM {quotedTableName} {where}";

        return Connection.CreateCommand(commandText, arguments);
    }

    public IDbCommand CreateInsertCommand(string tableName, Dictionary<string, object> columnsAndValues)
    {
        var arguments = new CommandArguments();
        var quotedTableName = QuoteObjectName(tableName);
        var columnList = (columnsAndValues.Keys.Select(QuoteObjectName)).JoinedBy(", ");
        var argumentList = (columnsAndValues.Keys.Select(ParameterName)).JoinedBy(", ");
        var commandText = $"INSERT INTO {quotedTableName} ({columnList}) VALUES {argumentList}";

        return Connection.CreateCommand(commandText, arguments);
    }

    public IDbCommand CreateSelectCommand(string tableName, Dictionary<string, object> key, IEnumerable<string>? columns = null)
    {
        var arguments = new CommandArguments();
        var quotedTableName = QuoteObjectName(tableName);
        var columnList = (columns?.Select(QuoteObjectName) ?? new[] { "*" }).JoinedBy(", ");
        var where = ExpandWhereText(key);
        var commandText = $"SELECT {columnList} FROM {quotedTableName} {where}";
        return Connection.CreateCommand(commandText, arguments);
    }

    public IDbCommand CreateUpdateCommand(string tableName, Dictionary<string, object> key, Dictionary<string, object> columnsAndValues)
    {
        var arguments = new CommandArguments();
        var quotedTableName = QuoteObjectName(tableName);
        var columnNames = columnsAndValues.Keys;
        var assignments = columnNames.Select(c => $"{QuoteObjectName(c)}={ParameterName(c)}");
        var where = ExpandWhereText(key);
        var commandText = $"UPDATE {quotedTableName} SET {assignments} {where}";
        return Connection.CreateCommand(commandText, arguments);
    }
}

