using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Artemis;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Artemis.Fact
{
    public class ArtemisFacts
    {
        public class Test
        {

        }

        [Fact]
        public void ESRequestTest()
        {

            ESHttpRequest es = new ESHttpRequest("172.10.252.123", "zfb-thirdparty_log_apihttp*", "doc", "9200");
            string json_query = @"
            {
                ""query"":{
                      ""match"":{
                            ""message.requestBody"":""1906252018009096""
                        }
                  }
            }";
            string strJsonResult = es.ExecuteQuery(json_query);
            var ob = JsonConvert.DeserializeObject<HttpLog<Message, Host>>(strJsonResult);
        }

        [Fact]

        public void ESEntryTest()
        {
            ArtemisESPower.Register(new Uri("http://172.10.252.123:9200"));
        }

        [Fact]

        public void ESEntryTestForPool()
        {
            ArtemisESPower.Register(new Uri[]
            {
                new Uri("http://172.10.252.123:9200"),
                new Uri("http://172.10.252.124:9200")
            });
        }

        [Fact]
        public void ESEntryForSingleToRequest()
        {
            ArtemisESPower.Register(new Uri("http://172.10.252.123:9200"));
            ESEntryRequest eSEntryRequest = new ESEntryRequest();
            IDictionary<string, string> dicWheres = new Dictionary<string, string>();
            dicWheres.Add("message.requestBody", "1906182018024713");
            var result = eSEntryRequest.ExecuteQuery<HttpLog<Message,Host>>("zfb-thirdparty_log_apihttp*", "doc", 0, 200, dicWheres);
        }

        [Fact]
        public void ESEntryForPoolToRequest()
        {
            ArtemisESPower.Register(new Uri[]
            {
                new Uri("http://172.10.252.123:9200"),
                new Uri("http://172.10.252.124:9200")
            });
            ESEntryRequest eSEntryRequest = new ESEntryRequest();
            IDictionary<string, string> dicWheres = new Dictionary<string, string>();
            dicWheres.Add("message.requestBody", "1906182018024713");
            var result = eSEntryRequest.ExecuteQuery<HttpLog<Message, Host>>("zfb-thirdparty_log_apihttp*", "doc", 0, 200, dicWheres);
        }

        [Fact]
        public async Task ESEntryForSingleToRequestAsync()
        {
            ArtemisESPower.Register(new Uri("http://172.10.252.123:9200"));
            ESEntryRequest eSEntryRequest = new ESEntryRequest();
            IDictionary<string, string> dicWheres = new Dictionary<string, string>();
            dicWheres.Add("message.requestBody", "1906182018024713");
            var result = await eSEntryRequest.ExecuteQueryAsync<HttpLog<Message, Host>>("zfb-thirdparty_log_apihttp*", "doc", 0, 200, dicWheres);
        }

        [Fact]
        public async Task ESEntryForPoolToRequestAsync()
        {
            ArtemisESPower.Register(new Uri[]
           {
                new Uri("http://172.10.252.123:9200"),
                new Uri("http://172.10.252.124:9200")
           });
            ESEntryRequest eSEntryRequest = new ESEntryRequest();
            IDictionary<string, string> dicWheres = new Dictionary<string, string>();
            dicWheres.Add("message.requestBody", "1906182018024713");
            var result = await eSEntryRequest.ExecuteQueryAsync<HttpLog<Message, Host>>("zfb-thirdparty_log_apihttp*", "doc", 0, 200, dicWheres);
        }
    }
}
