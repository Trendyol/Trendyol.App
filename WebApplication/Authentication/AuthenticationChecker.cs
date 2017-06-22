using Trendyol.App.Authentication;

namespace WebApplication.Authentication
{
    public class AuthenticationChecker : IAuthenticationChecker
    {
        public bool Check(string username, string password)
        {
            return username == "user" && password == "psw" ;
        }
    }
}