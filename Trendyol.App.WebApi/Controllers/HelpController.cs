using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Common.Logging;
using Trendyol.App.Domain.Objects;
using Trendyol.App.WebApi.HealthCheck;

namespace Trendyol.App.WebApi.Controllers
{
    [RoutePrefix("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HelpController : TrendyolApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<HelpController>();

        [HttpGet, Route("")]
        public IHttpActionResult Index()
        {
            return Redirect($"{Request.RequestUri.AbsoluteUri}help/index");
        }

        [HttpGet, Route("healthcheck")]
        public IHttpActionResult HealthCheck()
        {
            HealthCheckResponse response = new HealthCheckResponse();

            HealthCheckerContainer container = TrendyolApp.Instance.DataStore.GetData<HealthCheckerContainer>(Constants.HealthCheckerContainerDataKey);

            if (container == null || container.HealthCheckers.IsEmpty())
            {
                return Ok(response);
            }

            IHealthCheckerActivator healthCheckerActivator = TrendyolApp.Instance.GetHealthCheckerActivator();

            response.Results = new List<HealthCheckResult>();
            foreach (HealthChecker checker in container.HealthCheckers)
            {
                IHealthChecker checkerInstance = healthCheckerActivator.CreateHealthCheckerInstance(checker.HealthCheckerType);

                HealthCheckResult result;

                try
                {
                    result = checkerInstance.CheckHealth();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);

                    result = new HealthCheckResult();
                    result.Key = checker.Name;
                    result.IsCtirical = checker.IsCritical;
                    result.Message = ex.Message;
                    result.Success = false;
                }

                response.Results.Add(result);
            }

            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (response.Results.HasElements() && response.Results.Any(r => r.IsCtirical && r.Success == false))
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Content(statusCode, response);
        }
    }
}
