using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using Trendyol.App.WebApi.Models;

namespace Trendyol.App.WebApi.Controllers
{
    [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest", typeof(ErrorResponse))]
    public class TrendyolApiController : ApiController
    {
        protected IHttpActionResult InvalidRequest(string errorMessage)
        {
            return InvalidRequest(errorMessage, String.Empty);
        }

        protected IHttpActionResult InvalidRequest(string errorMessage, string errorCode)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.ErrorCode = errorCode;
            errorResponse.AddErrorMessage(errorMessage);

            return Content(HttpStatusCode.BadRequest, errorResponse);
        }
    }
}
