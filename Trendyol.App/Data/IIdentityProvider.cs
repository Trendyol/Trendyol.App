namespace Trendyol.App.Data
{
    public interface IIdentityProvider<TEntity, TId> : IIdentityProvider where TEntity : IEntity<TId>
    {
        TId Next();
    }

    public interface IIdentityProvider
    {
    }
}