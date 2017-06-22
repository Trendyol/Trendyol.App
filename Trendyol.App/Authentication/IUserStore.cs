using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Authentication
{
    public interface IUserStore
    {
        IUser GetUserByUsername(string username);
    }
}