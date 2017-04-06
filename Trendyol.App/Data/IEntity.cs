namespace Trendyol.App.Data
{
    public interface IEntity<out T>
    {
        T Id { get; }
    }
}