using System.Web.Mvc;
using Domain.Requests;
using Domain.Responses;
using Domain.Services;

namespace MvcApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : Controller
    {
        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [HttpGet, Route("")]
        public ActionResult Index()
        {
            QuerySamplesResponse response = _sampleService.QuerySamples(new QuerySamplesRequest());
            return View(response);
        }
    }
}