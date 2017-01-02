using System.Collections.Generic;
using Domain.Objects;
using Trendyol.App.Data;

namespace Domain.Repositories
{
    public interface ISampleRepository : IRepository
    {
        List<Sample> QuerySamples(string fields, string name);

        Sample CreateSample(string name);
    }
}