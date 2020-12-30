using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace AglieFrame.NoSql
{
    /// <summary>
    /// 官方文档https://www.elastic.co/guide/en/elasticsearch/client/net-api/6.x/nest-getting-started.html
    /// </summary>
    public class ElasticSearchService
    {
        private ElasticClient _client;
        public ElasticSearchService()
        {
            var settings = new ConnectionSettings(new Uri(ConfigHelper.GetConnStr("ElasticSearch"))) .DefaultIndex("TestIndex");
            _client = new ElasticClient(settings);
        }
        /// <summary>
        /// 数据写入es
        /// </summary>
        public bool IndexDoc<T>(T model) where T : class
        {
            var res= _client.IndexDocument(model);
            return res.IsValid;
        }
        public List<T> Search<T>() where T : class
        {
            var res = _client.Search<T>(s => s.From(0).Size(10));
            return res.Documents.ToList();
        }
        public bool Delete<T>(T model) where T : class
        {
            var res= _client.Delete<T>(model);
            return res.IsValid;
        }

        public ElasticClient GetClient()
        {
            return _client;

        }
    }
}
