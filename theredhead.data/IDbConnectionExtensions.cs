using System.Data;

namespace theredhead.data;

public static class IDbConnectionExtensions
{
    public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        var command = connection.CreateCommand();
        command.CommandText = commandText;

        if (arguments != null)
        {
            foreach(var kvp in arguments)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = kvp.Key;
                parameter.Value = kvp.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }

        return command;
    }
    public static IDataReader ExecuteReader(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        using var command = CreateCommand(connection, commandText, arguments);
        return command.ExecuteReader();
    }
    public static int ExecuteNonQuery(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        using var command = CreateCommand(connection, commandText, arguments);
        return command.ExecuteNonQuery();
    }
    public static DataTable ExecuteDataTable(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        using var reader = ExecuteReader(connection, commandText, arguments);

        var table = new DataTable();
        table.Load(reader);

        return table;
    }
    public static DataSet ExecuteDataSet(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        using var reader = ExecuteReader(connection, commandText, arguments);
        var dataSet = new DataSet();
        do
        {
            var table = new DataTable();
            table.Load(reader);
            dataSet.Tables.Add(table);
        }
        while (reader.NextResult());
        return dataSet;
    }
    public static IEnumerable<T> ExecuteLazyList<T>(this IDbConnection connection, string commandText, CommandArguments? arguments = null) where T : class, IDataReaderLoadable, new()
    {
        using var reader = ExecuteReader(connection, commandText, arguments);
        do
        {
            var item = new T();
            item.Load(reader);
            yield return item;
        }
        while (reader.Read());
    }
    public static IEnumerable<T> ExecuteArray<T>(this IDbConnection connection, string commandText, CommandArguments? arguments = null) where T : class, IDataReaderLoadable, new()
    {
        return ExecuteLazyList<T>(connection, commandText, arguments).ToArray();
    }
    public static T? ExecuteScalar<T>(this IDbConnection connection, string commandText, CommandArguments? arguments = null)
    {
        var result = CreateCommand(connection, commandText, arguments).ExecuteScalar();
        return (T?)Convert.ChangeType(result, typeof(T));
    }

}
