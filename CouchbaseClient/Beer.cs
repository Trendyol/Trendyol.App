using Trendyol.App.Couchbase.Abstractions;

namespace CouchbaseClient
{
    public class Beer : ICouchbaseEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}