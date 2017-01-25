namespace Trendyol.App.Data
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}