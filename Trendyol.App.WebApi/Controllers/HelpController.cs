using System.Web.Http;
using System.Web.Http.Description;

namespace Trendyol.App.WebApi.Controllers
{
    [RoutePrefix("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HelpController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Index()
        {
            return Redirect($"{Request.RequestUri.AbsoluteUri}help/index");
        }

        [HttpGet, Route("healthcheck")]
        public IHttpActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
