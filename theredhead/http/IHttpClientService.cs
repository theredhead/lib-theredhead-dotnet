using System;
using theredhead.remoting.serialization;

namespace theredhead.remoting.http
{
    /// <summary>
    /// Used to provide some implementation of http transport for the normal
    /// REST methods GET, POST, PUT, and DELETE
    /// </summary>
	public interface IHttpClientService : IHttpSynchronousClientService, IAsynchronousHttpClientService
    {
    }

    public interface IHttpSynchronousClientService : IDisposable
    {
        public HttpResponseMessage Get(string url);
        public HttpResponseMessage Delete(string url);
        public HttpResponseMessage Post(string url, HttpContent content);
        public HttpResponseMessage Post(string url, byte[] data);
        public HttpResponseMessage Post(string url, string data);
        public HttpResponseMessage Put(string url, HttpContent content);
        public HttpResponseMessage Put(string url, byte[] data);
        public HttpResponseMessage Put(string url, string data);
    }

    public interface IAsynchronousHttpClientService : IDisposable
    {
        public Task<HttpResponseMessage> GetAsync(string url);
        public Task<HttpResponseMessage> DeleteAsync(string url);
        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        public Task<HttpResponseMessage> PostAsync(string url, byte[] data);
        public Task<HttpResponseMessage> PostAsync(string url, string data);
        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content);
        public Task<HttpResponseMessage> PutAsync(string url, byte[] data);
        public Task<HttpResponseMessage> PutAsync(string url, string data);
    }
}

