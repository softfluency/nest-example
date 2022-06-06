using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Zapisnici;

var helper = new ElasticHelper();
var client = helper.LowLevelClient;

Console.WriteLine("Delete indicies");
var indicies = new List<string>() { "zapisnici_01", "zapisnici_02", "zapisnici_03" };
indicies.ForEach(async index => await helper.DeleteIndex(index));

// index a document from a JSON string, creating an index with auto-mapped properties
var zapisnici = new List<Tuple<string, string, string>>()
{
    Tuple.Create("zapisnici_01", "1", "{ \"broj\": \"RZ_0001\", \"katastarskaOpstina\":\"KO_Batajnica\", \"katastarskaParcela\":\"12345\"}"),
    Tuple.Create("zapisnici_01", "2", "{ \"broj\": \"RZ_0002\",\"katastarskaOpstina\":\"KO_Batajnica\",\"katastarskaParcela\":\"123456\"}"),
    Tuple.Create("zapisnici_01", "3", "{ \"broj\": \"RZ_0003\",\"katastarskaOpstina\":\"KO_Batajnica\",\"katastarskaParcela\":\"1234567\"}"),
    Tuple.Create("zapisnici_02", "4", "{ \"broj\": \"RZ_0004\",\"katastarskaOpstina\":\"KO_Batajnica\",\"katastarskaParcela\":\"23456\"}"),
    Tuple.Create("zapisnici_02", "5", "{ \"broj\": \"RZ_0005\",\"katastarskaOpstina\":\"KO_Batajnica\",\"katastarskaParcela\":\"123456\"}"),
    Tuple.Create("zapisnici_03", "6", "{ \"broj\": \"RZ_0006\",\"katastarskaOpstina\":\"KO_Batajnica\",\"katastarskaParcela\":\"123456\"}"),
    Tuple.Create("zapisnici_03", "7", "{ \"broj\": \"RZ_0007\",\"katastarskaOpstina\":\"KO_Ugrinovci\",\"katastarskaParcela\":\"123456\"}")
};

Console.WriteLine("Adding data");
foreach (var zapisnik in zapisnici)
{
    var result = await helper.AddDocument(zapisnik.Item1, zapisnik.Item2, zapisnik.Item3);
    Console.Write($"{zapisnik.Item1}, {zapisnik.Item2} ");
    Console.WriteLine(result ? "√" : "x");
}
Console.WriteLine();

Console.WriteLine("Press <enter> to query data");
Console.ReadLine();

var zapisniciQuery = @"
    {
        ""query"": {
            ""match"": {
                ""katastarskaOpstina"": ""KO_Ugrinovci""
            }
        }
    }";

var searchResponse = await helper.Search("zapisnici*", zapisniciQuery);
Console.WriteLine($"{searchResponse.Body}");

if (searchResponse == null)
{
    Console.WriteLine("No documents found");
} else
{
    dynamic stuff = JsonConvert.DeserializeObject(searchResponse.Body);

    var i = 0;
    foreach (var row in stuff.hits.hits)
    {
        Console.WriteLine(++i);
        // Console.WriteLine($"{row._source["broj"]}");
        Console.WriteLine($"{row}");
    }
}

Console.WriteLine();
Console.WriteLine("That`s all folks");