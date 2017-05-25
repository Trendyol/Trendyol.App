using System;
using System.Collections.Generic;
using System.Configuration;
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

                if (checkerInstance == null)
                {
                    throw new ConfigurationErrorsException($"There was a problem while creating healthchecker instance for type:{checker.HealthCheckerType.FullName}. Check if your type implements IHealthChecker interface.");
                }

                HealthCheckResult result = new HealthCheckResult();
                result.Key = checkerInstance.Key;
                result.IsCtirical = checkerInstance.IsCritical;

                try
                {
                    result.Success = checkerInstance.CheckHealth();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);

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
