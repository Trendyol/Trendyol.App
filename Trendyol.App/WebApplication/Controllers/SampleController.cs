using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
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

            return Ok(samples);
        }
    }
}