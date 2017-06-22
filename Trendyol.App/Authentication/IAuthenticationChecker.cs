namespace Trendyol.App.Authentication
{
    public interface IAuthenticationChecker
    {
        bool Check(string username, string password);
    }
}