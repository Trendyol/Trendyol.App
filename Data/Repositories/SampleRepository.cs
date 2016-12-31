using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Objects;
using Domain.Repositories;

namespace Data.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        public List<Sample> QuerySamples(string fields, string name)
        {
            List<Sample> samples;

            using (var context = new DataContext())
            {
                IQueryable<Sample> query = context.Samples;

                if (!String.IsNullOrEmpty(name))
                {
                    query = query.Where(s => s.Name == name);
                }

                samples = query.Select(fields).ToList();
            }

            return samples;
        }
    }
}