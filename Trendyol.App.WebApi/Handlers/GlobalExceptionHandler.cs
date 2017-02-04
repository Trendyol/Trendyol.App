using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Common.Logging;
using Trendyol.App.WebApi.Models;

namespace Trendyol.App.WebApi.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private static ILog _logger;

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            if (_logger == null)
            {
                _logger = LogManager.GetLogger<GlobalExceptionHandler>();
            }

            var exception = context.Exception;

            _logger.Error(exception.Message, exception);

            while (exception.InnerException != null) exception = exception.InnerException;

            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.AdditionalInfo = exception.Message;
            errorResponse.ErrorCode = "InternalServerError";
            errorResponse.AddErrorMessage("İşlem sırasında beklenmeyen bir hata oluştu.");

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);

            context.Result = new ResponseMessageResult(response);
        }
    }
}
