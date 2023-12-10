using theredhead.remoting.http;
using theredhead.remoting.serialization;
using Newtonsoft.Json.Linq;

namespace theredhead.remoting.tests
{
    internal struct TodoItem
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

    public class HttpClientServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HttpClientService_Can_Perform_Synchronous_Get_Request()
        {
            using var service = new HttpClientService();
            var response = service.Get("http://jsonplaceholder.typicode.com/todos/1");
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }
        [Test]
        public void HttpClientService_Can_Perform_Synchronous_Post_Request()
        {
            using var service = new HttpClientService();
            var myTodo = @"
            {{
                ""title"": ""Buy Milk"",
                ""completed"": false,
                ""userId"": 1,
            }}";
            var response = service.Post("http://jsonplaceholder.typicode.com/todos/", myTodo);

            Assert.That(response.IsSuccessStatusCode, Is.True);
            var json = response.GetBodyString();
            var jobj = JObject.Parse(json);
            var id = jobj.Value<int>("id");

            Assert.That(id, Is.Positive);
        }

        [Test]
        public async Task HttpClientService_Can_Perform_Asynchronous_Get_Request()
        {
            using var service = new HttpClientService();

            var response = await service.GetAsync("http://jsonplaceholder.typicode.com/todos/1");
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }
        [Test]
        public async Task HttpClientService_Can_Perform_Asynchronous_Post_Request()
        {
            using var service = new HttpClientService();
            var myTodo = @"
            {{
                ""title"": ""Buy Milk"",
                ""completed"": false,
                ""userId"": 1,
            }}";
            var response = await service.PostAsync("http://jsonplaceholder.typicode.com/todos/", myTodo);

            Assert.That(response.IsSuccessStatusCode, Is.True);
            var json = response.GetBodyString();
            var jobj = JObject.Parse(json);
            var id = jobj.Value<int>("id");

            Assert.That(id, Is.Positive);
        }
    }
}


