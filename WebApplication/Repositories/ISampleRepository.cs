using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface ISampleRepository
    {
        List<Sample> GetSamples(string fields = null, string name = null);
    }
}