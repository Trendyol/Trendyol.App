using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trendyol.App.Domain.Abstractions
{
    public interface IUser
    {
        string Username { get; set; }
        List<string> Roles { get; set; }
    }
}