using System.Data;

namespace theredhead.data;

public class DataRecord : IDataReaderLoadable
{
    public Guid RecordId {get; private set;} = Guid.NewGuid();
    public bool IsModified => ChangedFields.Any();
    public bool IsNew => !storedValues.Keys.Any();

    private Dictionary<string, object?> storedValues = [];
    private Dictionary<string, object?> unsavedValues = [];
    protected IEnumerable<string> ChangedFields {
        get 
        {
            return 
                from u in unsavedValues
                join s in storedValues on u.Key equals s.Key
                where u.Value != s.Value
                select u.Key;
        }
    }

    public object? this[string field] 
    {
        get 
        {
            if (unsavedValues.ContainsKey(field)) {
                return unsavedValues[field];
            }
            else if (storedValues.ContainsKey(field)) {
                return storedValues[field];
            }
            return null;
        }
        set 
        {
            unsavedValues[field] = value;
        }
    }

    public void Load(IDataReader reader)
    {
        for(var i= 0; i < reader.FieldCount; i ++)
        {
            var field = reader.GetName(i);
            storedValues[field] = reader.GetValue(i);
        }
    }

    public Dictionary<string, object?> ToDictionary()
    {
        var result = new Dictionary<string, object?>();
        var fields = new HashSet<string>(storedValues.Keys.Concat(unsavedValues.Keys));
        
        foreach(var field in fields) 
        {
            result[field] = this[field];
        }

        return result;
    }

}
