using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Trendyol.App.WebApi.Handlers
{
    public class RequestCorrelationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> correlationValue;
            if (request.Headers.TryGetValues("correlationId", out correlationValue))
            {
                Guid correlationId;
                Guid.TryParse(correlationValue.FirstOrDefault(), out correlationId);

                System.Diagnostics.Trace.CorrelationManager.ActivityId = correlationId;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}