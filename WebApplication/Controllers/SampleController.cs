using System.Collections.Generic;
using System.Web.Http;
using Common.Logging;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Requests;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            Logger.Trace("Fetching samples.");

            var samples = new List<SampleViewModel>();

            using (var context = new DataContext())
            {
                var query = context.Samples;

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