using System;
using System.Net.Http;
using System.Net.Http.Headers;
using theredhead.remoting.serialization;

namespace theredhead.remoting.http
{
	public partial class HttpClientService : IHttpClientService
    {
        public ISerializer Serializer { get; set; } = new JsonSerializer();

        protected virtual void Prepare(HttpRequestMessage request)
        {
        }

        protected virtual HttpRequestMessage CreateRequestMessage(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);
            return request;
        }

        public void Dispose()
        {
        }
    }
}

