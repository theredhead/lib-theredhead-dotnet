using System.Data;

namespace theredhead.data;

public interface IDataReaderLoadable
{
    void Load(IDataReader reader);
}