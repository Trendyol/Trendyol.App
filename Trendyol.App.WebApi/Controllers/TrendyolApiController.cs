using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Swashbuckle.Swagger.Annotations;
using Trendyol.App.Data;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Responses;
using Trendyol.App.WebApi.Models;

namespace Trendyol.App.WebApi.Controllers
{
    [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest", typeof(ErrorResponse))]
    [SwaggerResponseRemoveDefaults]
    public class TrendyolApiController : ApiController
    {
        protected new IHttpActionResult Ok<T>(T content)
        {
            if (typeof(BaseResponse).IsAssignableFrom(typeof(T)))
            {
                BaseResponse baseResponse = content as BaseResponse;

                if (baseResponse != null && baseResponse.HasError)
                {
                    if (baseResponse.Messages.Any(m => m.Content == Constants.MessageTypes.NotFound))
                    {
                        return Content(HttpStatusCode.NotFound, content);
                    }
                    return Content(HttpStatusCode.BadRequest, content);
                }
            }

            return base.Ok(content);
        }

        protected IHttpActionResult Page<T>(IPage<T> page)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page), @"You need to provide a valid page object.");
            }

            if (page.Size == 0)
            {
                return Ok(page.Items);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, page.Items);

            response.Headers.Add("X-Paging-Index", page.Index.ToString());
            response.Headers.Add("X-Paging-Size", page.Size.ToString());
            response.Headers.Add("X-Paging-TotalCount", page.TotalCount.ToString());
            response.Headers.Add("X-Paging-TotalPages", page.TotalPages.ToString());
            response.Headers.Add("X-Paging-HasPreviousPage", page.HasPreviousPage.ToString().ToLowerInvariant());
            response.Headers.Add("X-Paging-HasNextPage", page.HasNextPage.ToString().ToLowerInvariant());
            response.Headers.Add("Link", GetLinkHeaderForPageResult(page));
            response.Headers.Add("Access-Control-Expose-Headers", "X-Paging-Index, X-Paging-Size, X-Paging-TotalCount, X-Paging-TotalPages, X-Paging-HasPreviousPage, X-Paging-HasNextPage, Link");

            return ResponseMessage(response);
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

        protected IHttpActionResult Conflict(string errorMessage)
        {
            return Conflict(errorMessage, String.Empty);
        }

        protected IHttpActionResult Conflict(string errorMessage, string errorCode)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.ErrorCode = errorCode;
            errorResponse.AddErrorMessage(errorMessage);

            return Content(HttpStatusCode.Conflict, errorResponse);
        }

        protected IHttpActionResult Forbidden(string errorMessage)
        {
            return Forbidden(errorMessage, String.Empty);
        }

        protected IHttpActionResult Forbidden(string errorMessage, string errorCode)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.ErrorCode = errorCode;
            errorResponse.AddErrorMessage(errorMessage);

            return Content(HttpStatusCode.Forbidden, errorResponse);
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

        protected IHttpActionResult InternalServerError(string errorMessage, string additionalInfo = null)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.ErrorCode = "InternalServerError";
            errorResponse.AdditionalInfo = additionalInfo;
            errorResponse.AddErrorMessage(errorMessage);

            return Content(HttpStatusCode.InternalServerError, errorResponse);
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

        private string GetLinkHeaderForPageResult<T>(IPage<T> page)
        {
            var requestUrl = Request.RequestUri.ToString();
            string headerValue = String.Empty;

            if (page.HasNextPage)
            {
                var nextPageUrl = requestUrl.Replace($"page={page.Index}", $"page={page.Index+1}");
                headerValue += $"<{nextPageUrl}>; rel=\"next\",";
            }

            var lastPageUrl = requestUrl.Replace($"page={page.Index}", $"page={page.TotalPages}");
            headerValue += $"<{lastPageUrl}>; rel=\"last\",";

            var firstPageUrl = requestUrl.Replace($"page={page.Index}", "page=1");
            headerValue += $"<{firstPageUrl}>; rel=\"first\",";

            if (page.HasPreviousPage)
            {
                var previousPageUrl = requestUrl.Replace($"page={page.Index}", $"page={page.Index - 1}");
                headerValue += $"<{previousPageUrl}>; rel=\"prev\",";
            }

            if (!String.IsNullOrEmpty(headerValue))
            {
                headerValue = headerValue.TrimEnd(',');
            }

            return headerValue;
        }
    }
}
