using System.Collections.Generic;
using System.Linq;
using Jarvis.Filtering;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        public List<Sample> GetSamples(string fields = null, string name = null)
        {
            List<Sample> samples;

            using (var context = new Context())
            {
                samples = context
                    .Samples
                    .Where(new QueryParameter("Name", name))
                    .Select(fields)
                    .ToList();
            }

            return samples;
        }
    }
}