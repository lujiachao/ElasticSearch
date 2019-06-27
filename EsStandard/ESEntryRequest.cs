using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis
{
    public class ESEntryRequest
    {
        public ESEntryRequest()
        {

        }

        /// <summary>
        /// 单节点处理方案
        /// </summary>
        /// <param name="httpUri"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        public ElasticClient CreateConnectionSingle(Uri httpUri, string defaultIndex = "default")
        {
            var settings = new ConnectionSettings(httpUri).DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            return client;
        }

        /// <summary>
        /// 多节点集群解决方案
        /// </summary>
        /// <param name="httpUris"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        public ElasticClient CreateConnectionPool(Uri[] httpUris, string defaultIndex = "default")
        {
            var pool = new StaticConnectionPool(httpUris);
            var settings = new ConnectionSettings(pool).DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            return client;
        }

        public List<T> ExecuteQuery<T>(string indexName,string type,int startIndex, int endIndex,IDictionary<string, string> dicWheres = null, IDictionary<string, SortOrder> dicSorts = null) where T : class
        {
            ElasticClient client = new ElasticClient();
            switch ((int)ArtemisOption.connectionType)
            {
                case 1:
                    client = CreateConnectionSingle(ArtemisOption.singleUri);
                    break;
                case 2:
                    client = CreateConnectionPool(ArtemisOption.poolUri);
                    break;
                default:

                    break;
            }
            var searchRequest = QueryRequest(indexName, type, dicWheres, startIndex, endIndex, dicSorts);
            var httpLog = ExecuteSearch<T>(client, searchRequest);
            return httpLog;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string indexName, string type, int startIndex, int endIndex, IDictionary<string, string> dicWheres = null, IDictionary<string, SortOrder> dicSorts = null) where T : class
        {
            ElasticClient client = new ElasticClient();
            switch ((int)ArtemisOption.connectionType)
            {
                case 1:
                    client = CreateConnectionSingle(ArtemisOption.singleUri);
                    break;
                case 2:
                    client = CreateConnectionPool(ArtemisOption.poolUri);
                    break;
                default:

                    break;
            }
            var searchRequest = QueryRequest(indexName, type, dicWheres, startIndex, endIndex, dicSorts);
            return await ExecuteSearchAsync<T>(client, searchRequest);
        }

        public SearchRequest QueryRequest(string indexName, string type, IDictionary<string, string> dicWheres = null, int startIndex = -1, int endIndex = -1, IDictionary<string, SortOrder> dicSorts = null)
        {
            SearchRequest sr = new SearchRequest(indexName, type);
            if (dicWheres != null)
            {
                foreach (var dicWhere in dicWheres)
                {
                    TermQuery tq = new TermQuery();
                    tq.Field = dicWhere.Key;
                    tq.Value = dicWhere.Value;
                    sr.Query = tq;
                }
            }
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
        }

        [Obsolete]
        public SearchRequest QueryRequestOld(string indexName, string type, IDictionary<string, string> dicWheres = null, int startIndex = -1, int endIndex = -1, IDictionary<string, SortOrder> dicSorts = null)
        {
            SearchRequest sr = new SearchRequest(indexName, type);
            if (dicWheres != null)
            {
                foreach (var dicWhere in dicWheres)
                {
                    TermQuery tq = new TermQuery();
                    tq.Field = dicWhere.Key;
                    tq.Value = dicWhere.Value;
                    sr.Query = tq;
                }
            }
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
        }

        public List<T> ExecuteSearch<T>(ElasticClient client, SearchRequest sr) where T : class
        {
            var result = client.Search<T>(sr);
            return result.Documents.ToList();
        }

        public async Task<List<T>> ExecuteSearchAsync<T>(ElasticClient client, SearchRequest sr) where T : class
        {
            var result = await client.SearchAsync<T>(sr);
            return result.Documents.ToList();
        }
    }
}
