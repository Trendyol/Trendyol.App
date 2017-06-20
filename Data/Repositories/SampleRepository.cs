using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Objects;
using Domain.Repositories;
using Trendyol.App.Data;
using Trendyol.App.EntityFramework.Extensions;
using Trendyol.App.EntityFramework.Mapping;

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

            using (var context = new SampleDataContext())
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
            sample.Size = 0;

            using (var context = new SampleDataContext())
            {
                context.Samples.Add(sample);
                context.SaveChanges();
            }

            return sample;
        }
    }
}