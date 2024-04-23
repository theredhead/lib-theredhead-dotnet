using System.Data;
using theredhead.data.sql;

namespace theredhead.data;

/// <summary>
/// Represents a repository for managing data records of type T.
/// </summary>
/// <typeparam name="T">The type of data record.</typeparam>
public class DataRecordRepository<T> where T : DataRecord, new()
{
    private List<T> records = new();

    /// <summary>
    /// Creates a new instance of the data record.
    /// </summary>
    /// <returns>The created data record.</returns>
    public T CreateRecord() 
    {
        var record = new T();
        return record;
    }

    /// <summary>
    /// Checks if the repository contains the specified data record.
    /// </summary>
    /// <param name="record">The data record to check.</param>
    /// <returns>True if the repository contains the data record, otherwise false.</returns>
    public bool Contains(T record) 
    {
        return records.Contains(record);
    }

    /// <summary>
    /// Adds a data record to the repository.
    /// </summary>
    /// <param name="record">The data record to add.</param>
    public void Add(T record) 
    {
        records.Add(record);
    }

    /// <summary>
    /// Removes a data record from the repository.
    /// </summary>
    /// <param name="record">The data record to remove.</param>
    public void Remove(T record)
    {
        records.Remove(record);
    }

    /// <summary>
    /// Retrieves all data records that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate used to filter the data records.</param>
    /// <returns>An enumerable collection of data records that satisfy the predicate.</returns>
    public IEnumerable<T> Where(Predicate<T> predicate) 
    {
        foreach( var record in records.ToArray() )
        {
            if (predicate(record))
            {
                yield return record;
            }
        }
    }
}

public class ConnectedDataRecordRepository<T> : DataRecordRepository<T> where T : DataRecord, new()
{
    public  IDbConnection Connection {get; init;}
    public string TableName {get; init;}
    public ConnectedDataRecordRepository(IDbConnection connection, string tableName)
    {
        Connection = connection;
        TableName = tableName;
    }

    public void LoadAll() 
    {
        var factory = Connection.GetCommandFactory();
        var command = factory.CreateSelectCommand(TableName, null);
        foreach(var record in command.ExecuteLazyList<T>())
        {
            Add(record);
        }
    }
    public IEnumerable<T> GetNewRecords() {
        return Where(r => r.IsNew);
    }
    public IEnumerable<T> GetModifiedRecords() {
        return Where(r => r.IsModified);
    }

    public virtual void Save() 
    {
        var records = GetNewRecords().Concat(GetModifiedRecords()).ToArray();
        Save(records);
    }

    public virtual void Save(IEnumerable<T> records) 
    {
        foreach(var record in records) {
            Save(record);
        }
    }
    
    public virtual void Save(T record) 
    {
        if (record.IsNew) {
            Insert(record);
        } else if (record.IsModified) {
            Update(record);
        }
    }

    protected virtual Dictionary<string, object> Key(T record)
    {
        return new Dictionary<string, object>() {
            { "Id", record.RecordId }
        };
    }

    protected virtual void Update(T record)
    {
        var factory = Connection.GetCommandFactory();
        var dict = record.ToDictionary();
        var command = factory.CreateUpdateCommand(TableName, Key(record), dict);
        command.ExecuteNonQuery();
    }

    protected virtual void Insert(T record)
    {
        var factory = Connection.GetCommandFactory();
        var dict = record.ToDictionary();
        var command = factory.CreateInsertCommand(TableName, dict);
        command.ExecuteNonQuery();
    }
}
