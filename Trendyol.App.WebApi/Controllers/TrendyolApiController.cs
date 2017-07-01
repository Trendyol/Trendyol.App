using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Swashbuckle.Swagger.Annotations;
using Trendyol.App.Data;
using Trendyol.App.Domain.Responses;
using Trendyol.App.WebApi.Models;

namespace Trendyol.App.WebApi.Controllers
{
    [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest", typeof(ErrorResponse))]
    public class TrendyolApiController : ApiController
    {
        protected new IHttpActionResult Ok<T>(T content)
        {
            if (typeof(BaseResponse).IsAssignableFrom(typeof(T)))
            {
                BaseResponse baseResponse = content as BaseResponse;

                if (baseResponse != null && baseResponse.HasError)
                {
                    return Content(HttpStatusCode.BadRequest, content);
                }
            }

            return base.Ok(content);
        }

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

        protected IHttpActionResult Created(object returnValue)
        {
            BaseResponse baseResponse = returnValue as BaseResponse;

            if (baseResponse != null && baseResponse.HasError)
            {
                return Content(HttpStatusCode.BadRequest, returnValue);
            }

            string id = GetIdFromReturnValue(returnValue);

            if (String.IsNullOrEmpty(id))
            {
                return Created(String.Empty, returnValue);
            }

            string url = Request.RequestUri.AbsoluteUri;
            return Created($"{url.TrimEnd('/')}/{id}", returnValue);
        }

        protected IHttpActionResult Deleted(object returnValue = null)
        {
            if (returnValue == null)
            {
                return Content(HttpStatusCode.NoContent, returnValue);
            }

            return Ok(returnValue);
        }

        private string GetIdFromReturnValue(object returnValue)
        {
            if (returnValue != null)
            {
                PropertyInfo propertyInfo = returnValue.GetType().GetProperty("Id");

                if (propertyInfo != null)
                {
                    object idValue = propertyInfo.GetValue(returnValue, null);

                    if (idValue != null)
                    {
                        return idValue.ToString();
                    }
                }
            }

            return String.Empty;
        }
    }
}
