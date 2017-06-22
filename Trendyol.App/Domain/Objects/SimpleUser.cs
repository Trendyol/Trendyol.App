using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Domain.Objects
{
    public class SimpleUser : IUser
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
