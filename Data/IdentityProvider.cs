using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Objects;
using Trendyol.App.Data;

namespace Data
{
    public class IdentityProvider : IIdentityProvider<Sample, long>
    {
        public long Next()
        {
            return 1;
        }
    }
}
