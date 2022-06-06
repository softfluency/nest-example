using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Zapisnici
{
    public class ElasticHelper
    {
        public ElasticClient Client { get; set; }

        public IElasticLowLevelClient LowLevelClient 
        { 
            get
            {
                return this.Client.LowLevel;
            }
        }
        public ElasticHelper()
        {
            var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));

            // openssl x509 -fingerprint -sha256 -in config/certs/http_ca.crt
            var settings = new ConnectionSettings(pool)
                .CertificateFingerprint("5A:59:C9:5A:11:DB:EF:C8:CA:31:10:67:F1:74:B1:78:85:31:6E:4F:E5:11:EA:1B:4E:DD:AF:05:4E:9A:0B:EB")
                .BasicAuthentication("elastic", "kJkcftK2X8Skxp_Q6Ij_")
                .EnableApiVersioningHeader()
                .DefaultIndex("books");

            this.Client = new ElasticClient(settings);            
        }               

        public async Task<bool> AddDocument(string index, string id, string json)
        {
            var indexResponse = await LowLevelClient.IndexAsync<StringResponse>(index, id, PostData.String(json));
            return indexResponse.Success;
        }

        public async Task<VoidResponse> DeleteIndex(string index)
        {
            return await LowLevelClient.Indices.DeleteAsync<VoidResponse>(index);
        }
        
        public async Task<StringResponse> Search(string index, string query)
        {
            return await LowLevelClient.SearchAsync<StringResponse>(index, query);
        }
    }
}
