using System.Threading;

namespace Trendyol.App.WebApi.Filters
{
    public interface IAuthenticationChecker
    {
        bool Check(string username, string password, CancellationToken token);
    }
}