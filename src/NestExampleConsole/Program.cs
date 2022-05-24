using Elasticsearch.Net;
using Nest;
using NestExampleConsole;
using System.Text.Json;

async Task<bool> AddDocument(ElasticClient client, Document document)
{
    // var indexResponse = client.IndexDocument(document);
    var asyncIndexResponse = await client.IndexDocumentAsync(document);
    return asyncIndexResponse.IsValid;
}

void WriteDocuments (IReadOnlyCollection<Document>? documents)
{
    if (documents != null)
    {
        foreach (var d in documents)
        {
            Console.WriteLine($"{d.Type} {d.No} {d.Date}");
        }
    }
}

var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));

// openssl x509 -fingerprint -sha256 -in config/certs/http_ca.crt
var settings = new ConnectionSettings(pool)
    .CertificateFingerprint("5A:59:C9:5A:11:DB:EF:C8:CA:31:10:67:F1:74:B1:78:85:31:6E:4F:E5:11:EA:1B:4E:DD:AF:05:4E:9A:0B:EB")
    .BasicAuthentication("elastic", "kJkcftK2X8Skxp_Q6Ij_")
    .EnableApiVersioningHeader()
    .DefaultIndex("documents");
// .DefaultIndex("kibana_sample_data_ecommerce");

var client = new ElasticClient(settings);


//var document = new Document
//{
//    Type = "Ugovor",
//    No = "U0002",
//    Description = "Neki drugi ugovor",
//    Date = DateTime.Now
//};

//var addResult = await AddDocument(client, document);
//Console.WriteLine($"Result {addResult}");

var result = await client.Indices.GetAsync(new GetIndexRequest(Indices.All));

Console.WriteLine("Indices");
Console.WriteLine(JsonSerializer.Serialize(result.Indices.Keys));


var searchResponse = client.Search<Document>(s => s
    .From(0)
    .Size(10)
    .Query(q => q
         .Match(m => m
            .Field(f => f.Type)
            .Query("Ugovor")
         )
    )
);

var documents = searchResponse.Documents;

WriteDocuments(documents);


Console.WriteLine("----");
// https://localhost:9200/documents?pretty
// https://localhost:9200/documents/search?pretty
// https://localhost:9200/documents/_search?pretty

var searchRN = client.Search<Document>(s => s
    .From(0)
    .Size(10)
    .Query(q => q
         .Match(m => m
            .Field(f => f.No)
            .Query("RN0001")
         )
    )
);

var rns = searchRN.Documents;

WriteDocuments(rns);