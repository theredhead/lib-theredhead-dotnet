using System;
namespace theredhead.remoting.http
{
	public partial class HttpClientService
	{
        protected async Task<HttpResponseMessage> PerformRequestAsync(HttpRequestMessage request)
        {
            Prepare(request);
            using (var client = new HttpClient())
            {
                return await client.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url) => await PerformRequestAsync(CreateRequestMessage(HttpMethod.Get, url));
        public async Task<HttpResponseMessage> DeleteAsync(string url) => await PerformRequestAsync(CreateRequestMessage(HttpMethod.Delete, url));

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var request = CreateRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            return await PerformRequestAsync(request);
        }
        public async Task<HttpResponseMessage> PostAsync(string url, byte[] data) => await PostAsync(url, new ByteArrayContent(data));
        public async Task<HttpResponseMessage> PostAsync(string url, string data) => await PostAsync(url, new StringContent(data));

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            var request = CreateRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            return await PerformRequestAsync(request);
        }
        public async Task<HttpResponseMessage> PutAsync(string url, byte[] data) => await PutAsync(url, new ByteArrayContent(data));
        public async Task<HttpResponseMessage> PutAsync(string url, string data) => await PutAsync(url, new StringContent(data));
    }
}

