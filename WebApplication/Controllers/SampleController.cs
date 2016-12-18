using System.Collections.Generic;
using System.Web.Http;
using Common.Logging;
using WebApplication.Models;

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
            var samples = new List<SampleViewModel>();

            samples.Add(new SampleViewModel()
            {
                Id = 1,
                Name = "Sample1"
            });

            samples.Add(new SampleViewModel()
            {
                Id = 2,
                Name = "Sample2"
            });

            Logger.Debug("test");

            return Ok(samples);
        }
    }
}