using System;
using theredhead.remoting.serialization;

namespace theredhead.remoting.http
{
	public static class HttpResponseMessageExtensions
	{
		public static T As<T>(this HttpResponseMessage response) 
			=> As<T>(response, new JsonSerializer());
		
		public static T As<T>(this HttpResponseMessage response, ISerializer serializer) 
			=> serializer.Deserialize<T>(response.GetBodyString());
		
		public static string GetBodyString(this HttpResponseMessage response)
		{
			using var reader = new StreamReader(response.Content.ReadAsStream());
			return reader.ReadToEnd();
		}
	}
}

