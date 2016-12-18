using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Common.Logging;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Requests;
using Jarvis.Filtering;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        [Route("")]
        [HttpGet]
        public IHttpActionResult Filter(string fields = null, string name = null)
        {
            Logger.Trace("Fetching samples.");

            var samples = new List<SampleViewModel>();

            using (var context = new DataContext())
            {
                var query = context.Samples.Where(new QueryParameter("Name", name)).Select(fields);

                foreach (Sample sample in query)
                {
                    samples.Add(new SampleViewModel()
                    {
                        Id = sample.Id,
                        Name = sample.Name
                    });
                }
            }

            return Ok(samples);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(long id, string fields = null)
        {
            Logger.Trace($"Fetching sample by id: {id}.");

            Sample sample;

            using (var context = new DataContext())
            {
                sample = context.Samples.Where(s => s.Id == id).Select(fields).FirstOrDefault();
            }

            if (sample == null)
            {
                throw new HttpException(404, "");
            }

            return Ok(sample);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            Sample sample = new Sample();
            sample.Name = request.Name;

            using (var context = new DataContext())
            {
                context.Samples.Add(sample);
                context.SaveChanges();
            }

            return Ok(sample);
        }
    }
}