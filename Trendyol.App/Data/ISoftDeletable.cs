namespace Trendyol.App.Data
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}