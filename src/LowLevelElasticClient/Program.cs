using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));

// openssl x509 -fingerprint -sha256 -in config/certs/http_ca.crt
var settings = new ConnectionSettings(pool)
    .CertificateFingerprint("5A:59:C9:5A:11:DB:EF:C8:CA:31:10:67:F1:74:B1:78:85:31:6E:4F:E5:11:EA:1B:4E:DD:AF:05:4E:9A:0B:EB")
    .BasicAuthentication("elastic", "kJkcftK2X8Skxp_Q6Ij_")
    .EnableApiVersioningHeader()
    .DefaultIndex("books");

var clientNest = new ElasticClient(settings);
var client = clientNest.LowLevel;


var json = "{\"Id\":1,\"Title\":\"Pro .NET Memory Management\",\"ISBN\":\"978-1-4842-4026-7\"}";

// index a document from a JSON string, creating an index with auto-mapped properties
// var indexResponse = await client.IndexAsync<StringResponse>("books", "1", PostData.String(json));

//if (indexResponse.Success)
//{
//    await Task.Delay(10);

//    // after a short delay, try to search for a book with a specific ISBN
//    var searchResponse = await client.SearchAsync<StringResponse>("books", PostData.Serializable(new
//    {
//        query = new
//        {
//            match = new
//            {
//                isbn = new
//                {
//                    query = "978-1-4842-4026-7"
//                }
//            }
//        }
//    }));

//    if (searchResponse.Success)
//    {
//        Console.WriteLine(searchResponse.Body);
//        Console.WriteLine();
//    }
//}
//else
//{
//    Console.WriteLine("Eroriška 1 😒");
//}

// var bookToIndex = new Book(2, "Pro .NET Benchmarking", "978-1-4842-4940-6");

// index another book, this time serializing an object
// var indexResponse = await client.IndexAsync<StringResponse>("books", bookToIndex.Id.ToString(), PostData.Serializable(bookToIndex));

//if (indexResponse.Success)
//{
//    await Task.Delay(10);

//    // after a short delay, get the book back by its ID
//    var searchResponse = await client.GetAsync<DynamicResponse>("books", bookToIndex.Id.ToString());

//    if (searchResponse.Success)
//    {
//        // access the title by path notation from the dynamic response
//        Console.WriteLine($"Title: {searchResponse.Get<string>("_source.Title")}");
//    }
//}
//else
//{
//    Console.WriteLine("Eroriška 2 😒");
//}

// clean up by removing the index
// await client.Indices.DeleteAsync<VoidResponse>("books");

var zapisniciQuery = @"
    {
        ""query"": {
            ""match"": {
                ""katastarskaOpstina"": ""KO_Batajnica""
            }
        }
    }";

var searchResponse = await client.SearchAsync<StringResponse>("zapisnici*", zapisniciQuery);
Console.WriteLine($"{searchResponse.Body}");

dynamic stuff = JsonConvert.DeserializeObject(searchResponse.Body);

// Console.WriteLine($"Hits {stuff.hits.hits[0]}");

var i = 0;
foreach(var row in stuff.hits.hits)
{
    Console.WriteLine(++i);
    Console.WriteLine($"{row._source["broj"]}");    
    // Console.WriteLine($"{row}");
}


// BOOKS Search sample
//string searchJson = @"
//    {
//        ""query"": {
//            ""match"": {
//                ""ISBN"": ""978-1-4842-4026-7""
//            }
//        }
//    }";

//var searchString2 = PostData.Serializable(new
//{
//    query = new
//    {
//        match = new
//        {
//            isbn = new
//            {
//                query = "978-1-4842-4026-7"
//            }
//        }
//    }
//});

//// if index is created search
//var searchResponse = await client.SearchAsync<StringResponse>("books", searchJson);

//Console.WriteLine($"REQUEST 1: {searchJson}");
//Console.WriteLine($"RESPONSE 1: {searchResponse.Body}");
//if (searchResponse.Success)
//{    
//    Console.WriteLine();
//}

//// var searchResponse2 = await client.SearchAsync<StringResponse>("books", searchString2);
//var searchResponse2 = await client.SearchAsync<StringResponse>("books", searchString2);
//// client.LowLevel.Search<string>(query);

//if (searchResponse2.Success)
//{
//    Console.WriteLine($"REQUEST 2: {searchString2}");
//    Console.WriteLine($"RESPONSE 2: {searchResponse2.Body}");
//    Console.WriteLine();
//}


//// Using C# 9 record to define our book DTO
//internal record Book(int Id, string Title, string ISBN);