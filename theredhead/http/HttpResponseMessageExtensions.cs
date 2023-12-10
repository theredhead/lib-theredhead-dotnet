using System;
namespace theredhead.remoting.http
{
	public static class HttpResponseMessageExtensions
	{
		static public string GetBodyString(this HttpResponseMessage response)
		{
			using (var reader = new StreamReader(response.Content.ReadAsStream()))
			{
				return reader.ReadToEnd();
			}
		}
	}
}

