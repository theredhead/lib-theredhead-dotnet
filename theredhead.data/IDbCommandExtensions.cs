using System.Data;

namespace theredhead.data;

public static class IDbCommandExtensions {
    public static int ExecuteNonQuery(this IDbCommand command)
    {
        return command.ExecuteNonQuery();
    }

    public static DataTable ExecuteDataTable(this IDbCommand command)
    {
        using var reader = command.ExecuteReader();

        var table = new DataTable();
        table.Load(reader);

        return table;
    }

    public static DataSet ExecuteDataSet(this IDbCommand command)
    {
        using var reader = command.ExecuteReader();
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

    public static IEnumerable<T> ExecuteLazyList<T>(this IDbCommand command) where T : class, IDataReaderLoadable, new()
    {
        using var reader = command.ExecuteReader();

        do
        {
            var item = new T();
            item.Load(reader);
            yield return item;
        } while (reader.Read());
    }
}