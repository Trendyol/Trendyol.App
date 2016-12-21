using System.Web.Http;

namespace Trendyol.App.WebApi.Controllers
{
    [RoutePrefix("")]
    public class HelpController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Index()
        {
            return Redirect($"{Request.RequestUri.AbsoluteUri}help/index");
        }
    }
}
