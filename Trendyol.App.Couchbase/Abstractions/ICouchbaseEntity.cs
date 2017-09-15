using Trendyol.App.Data;

namespace Trendyol.App.Couchbase.Abstractions
{
    public interface ICouchbaseEntity<T> : IEntity<T>
    {
        string Type { get; set; }
    }
}