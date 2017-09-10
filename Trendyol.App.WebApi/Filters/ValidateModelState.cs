using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Trendyol.App.Domain.Responses;

namespace Trendyol.App.WebApi.Filters
{
    public class ValidateModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            BaseResponse errorResponse = new BaseResponse();

            if (actionContext.ActionArguments.Any(a => a.Value == null))
            {
                errorResponse.AddErrorMessage("Provided request body cannot be serialized.");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                foreach (ModelState modelState in actionContext.ModelState.Values)
                {
                    if (modelState.Errors.HasElements())
                    {
                        foreach (ModelError modelStateError in modelState.Errors)
                        {
                            errorResponse.AddErrorMessage(modelStateError.ErrorMessage);
                        }
                    }
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                return;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}