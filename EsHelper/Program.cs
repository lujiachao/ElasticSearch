using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EsHelper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var client = CreateConnectionSingle("http://172.10.252.123:9200");
            var client = CreateConnectionPool(new Uri[]
            {
                new Uri("http://172.10.252.123:9200"),
                new Uri("http://172.10.252.124:9200")
            });

            //IDictionary<string, string> dicWheres = new Dictionary<string, string>();
            //dicWheres.Add("message.requestBody", "22112507");
            IDictionary<string, SortOrder> dicSorts = new Dictionary<string, SortOrder>();
            dicSorts.Add("message.elapsedMilliseconds", SortOrder.Descending);
            var searchRequest = QueryRequest("zfb-thirdparty_log_apihttp*", "doc", "message.requestBody", new string[] { "22112507" }, 0, 200, dicSorts);
            var httpLog = ExecuteSearch(client, searchRequest);
        }

        /// <summary>
        /// 单节点处理方案
        /// </summary>
        /// <param name="httpUri"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        public static ElasticClient CreateConnectionSingle(string httpUri, string defaultIndex = "default")
        {
            var node = new Uri(httpUri);
            var settings = new ConnectionSettings(node).DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            return client;
        }

        /// <summary>
        /// 多节点集群解决方案
        /// </summary>
        /// <param name="httpUris"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        public static ElasticClient CreateConnectionPool(Uri[] httpUris,string defaultIndex = "default")
        {
            var pool = new StaticConnectionPool(httpUris);
            var settings = new ConnectionSettings(pool).DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            return client;
        }

        #region 词条查询
        public static SearchRequest QueryRequest(string indexName, string type,string key,string[] value, int startIndex = -1, int endIndex = -1, IDictionary<string, SortOrder> dicSorts = null)
        {
            SearchRequest sr = new SearchRequest(indexName, type);
            TermQuery tq = new TermQuery();
            tq.Field = key;
            if (value.Length <= 1)
            {
                tq.Value = value[0];
            }
            else
            {
                tq.Value = value;
            }
            sr.Query = tq;
            
            //查询数量 从startIndex条查询到endIndex条,索引从0开始
            if (startIndex > 0)
            {
                sr.From = startIndex;
            }
            if (endIndex > 0)
            {
                sr.Size = endIndex;
            }
            //sortn 根据字段升序和降序
            if (dicSorts != null)
            {
                foreach (var dicSort in dicSorts)
                {
                    ISort sort = new SortField { Field = dicSort.Key, Order = dicSort.Value };
                    sr.Sort = new List<ISort>();
                    sr.Sort.Add(sort);
                }
            }
            return sr;
            //source filter
            //sr.Source = new SourceFilter()
            //{
            //    Includes = new string[] { "@timestamp", "2019-05-14T01:38:10.711Z" },
            //    Excludes = new string[] { "@timestamp", "2019-05-14T01:37:37.571Z" }
            //};
        }

        //public static List<object> ExecuteSearch(ElasticClient client, SearchRequest sr)
        //{
        //    var result = client.Search<object>(sr);
        //    return result.Documents.ToList();
        //}

        public static List<HttpLog<Message, Host>> ExecuteSearch(ElasticClient client, SearchRequest sr)
        {
            var result = client.Search<HttpLog<Message, Host>>(sr);
            return result.Documents.ToList();
        }
        #endregion
    }
}
