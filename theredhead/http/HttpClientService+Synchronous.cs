using System;
using theredhead.remoting.serialization;

namespace theredhead.remoting.http
{
	public partial class HttpClientService
	{
        protected HttpResponseMessage PerformRequest(HttpRequestMessage request)
        {
            Prepare(request);
            using (var client = new HttpClient())
            {
                return client.Send(request);
            }
        }


        public HttpResponseMessage Get(string url) => PerformRequest(CreateRequestMessage(HttpMethod.Get, url));
        public HttpResponseMessage Delete(string url) => PerformRequest(CreateRequestMessage(HttpMethod.Delete, url));

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            var request = CreateRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            return PerformRequest(request);
        }
        public HttpResponseMessage Post(string url, byte[] data) => Post(url, new ByteArrayContent(data));
        public HttpResponseMessage Post(string url, string data) => Post(url, new StringContent(data));

        public HttpResponseMessage Put(string url, HttpContent content)
        {
            var request = CreateRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            return PerformRequest(request);
        }
        public HttpResponseMessage Put(string url, byte[] data) => Put(url, new ByteArrayContent(data));
        public HttpResponseMessage Put(string url, string data) => Put(url, new StringContent(data));
    }
}

