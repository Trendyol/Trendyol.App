using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Common.Logging;
using Trendyol.App.WebApi.Models;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Requests;
using WebApplication.Managers;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        private readonly ISampleManager _sampleManager;
        private readonly Context _context;
        private readonly ISampleRepository _sampleRepository;

        public SampleController(ISampleManager sampleManager, Context context, ISampleRepository sampleRepository)
        {
            _sampleManager = sampleManager;
            _context = context;
            _sampleRepository = sampleRepository;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Sample> Filter(string fields = null, string name = null)
        {
            Logger.Trace("Fetching samples.");

            List<Sample> samples = _sampleRepository.GetSamples(fields, name);

            return samples;
        }

        [Route("")]
        [HttpPost]
        public void Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            Sample sample = new Sample();
            sample.Name = request.Name;

            using (var context = new Context())
            {
                context.Samples.Add(sample);
                context.SaveChanges();
            }
        }

        [Route("{id}")]
        [HttpGet]
        public Sample Get(long id, string fields = null)
        {
            Logger.Trace($"Fetching sample by id: {id}.");

            Sample sample = _context.Samples.Where(s => s.Id == id).Select(fields).FirstOrDefault();

            if (sample == null)
            {
                throw new HttpException(404, "");
            }

            if (!_sampleManager.IsValid(sample))
            {
                throw new HttpException(400, "Invalid sample.");
            }

            return sample;
        }

        [Route("{id}")]
        [HttpPut]
        public void Put(long id, Sample sample)
        {
            Sample dbSample = _context.Samples.Find(id);

            if (dbSample == null)
            {
                throw new HttpException(404, "");
            }

            if (!_sampleManager.IsValid(sample))
            {
                throw new HttpException(400, "Invalid sample.");
            }

            dbSample.Name = sample.Name;
            dbSample.Size = sample.Size;
            _context.SaveChanges();
        }

        [Route("{id}")]
        [HttpPatch]
        public void Patch(long id, List<PatchParameter> parameters)
        {
            using (var context = new Context())
            {
                Sample sample = context.Samples.Find(id);

                foreach (PatchParameter patchParameter in parameters)
                {
                    patchParameter.Update(sample);
                }

                context.SaveChanges();
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(long id)
        {
            Sample sample = _context.Samples.Find(id);

            if (sample == null)
            {
                throw new HttpException(404, "");
            }

            _context.Samples.Remove(sample);
            _context.SaveChanges();
        }
    }
}