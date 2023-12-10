using System;
namespace theredhead.remoting.serialization
{
	public interface ISerializer : IDisposable
	{
        public string Serialize<T>(T subject);
        public T Deerialize<T>(string serializedSubject);
    }
}

