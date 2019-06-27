using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Artemis
{
    public class ESHttpRequest
    {
        string es_host;
        string es_port;
        string es_index;
        string es_type;
        private string url;
        private readonly IHttpClientFactory _httpClientFactory;
        public const string ContentTypeJson = "application/json";

        public ESHttpRequest(string host, string index, string type, string port, IHttpClientFactory httpClientFactory = null)
        {
            es_host = host;
            es_port = port;
            es_index = index;
            es_type = type;
            if (httpClientFactory != null)
            {
                _httpClientFactory = httpClientFactory;
            }

            string request_cache = "request_cache=true";
            url = string.Format("http://{0}:{1}/{2}/{3}/_search?{4}", es_host,es_port,es_index,es_type,request_cache);
        }

        public string ExecuteQuery(string json_query)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = ContentTypeJson;
            request.Method = "POST";
            request.Timeout = 1000 * 60;
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(json_query);
                sw.Flush();
                sw.Close();
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        public async Task<string> ExecuteQueryAsync(string clientName, string json_query)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var content = new StringContent(json_query);
            content.Headers.ContentType = new MediaTypeHeaderValue(ContentTypeJson);
            content.Headers.Add("charset", "utf-8");

            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            requestMessage.RequestUri = new Uri(url);

            var responseMessage = await client.SendAsync(requestMessage);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Post to es failed");
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}
