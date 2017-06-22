using System.Collections.Generic;
using Trendyol.App.Authentication;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Objects;

namespace WebApplication.Authentication
{
    public class UserStore : IUserStore
    {
        public IUser GetUserByUsername(string username)
        {
            return new SimpleUser
                   {
                       Username = "user",
                       Roles = new List<string>() {"simple"}
                   };
        }
    }
}