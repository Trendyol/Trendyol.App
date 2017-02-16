using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Objects;
using Domain.Repositories;
using Trendyol.App.Data;
using Trendyol.App.EntityFramework.Extensions;

namespace Data.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        private readonly IIdentityProvider<Sample, long> _identityProvider;

        public SampleRepository(IIdentityProvider<Sample, long> identityProvider)
        {
            _identityProvider = identityProvider;
        }

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

        public Sample CreateSample(string name)
        {
            Sample sample = new Sample();
            sample.Name = name;
            sample.CreatedOn = DateTime.UtcNow;
            sample.Size = 0;

            using (var context = new DataContext())
            {
                context.Samples.Add(sample);
                context.SaveChanges();
            }

            return sample;
        }
    }
}