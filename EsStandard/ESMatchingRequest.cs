using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artemis
{
    public class ESMatchingRequest
    {
        public ESMatchingRequest()
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

        //public List<T> ExecuteQuert<T>(string indexName, string type, int startIndex, int endIndex, IDictionary<string, string> dicWheres = null, IDictionary<string, SortOrder> dicSorts = null) where T : class
        //{

        //}

        public SearchRequest QueryRequest(string indexName, string type, IDictionary<string, string> dicWheres = null, int startIndex = -1, int endIndex = -1, IDictionary<string, SortOrder> dicSorts = null)
        {
            SearchRequest sr = new SearchRequest(indexName, type);
            if (dicWheres != null)
            {
                MatchQuery mq = new MatchQuery();
                foreach (var dicWhere in dicWheres)
                {
                    mq.Field= new Field(dicWhere.Key);
                    mq.Query = mq.Query + " " + dicWhere.Value;
                    mq.MinimumShouldMatch = 2;
                    mq.Operator = Operator.And;
                    sr.Query = mq;
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
    }
}
