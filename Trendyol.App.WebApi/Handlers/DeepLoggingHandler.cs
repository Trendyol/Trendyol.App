using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Trendyol.App.WebApi.DeepLogging;

namespace Trendyol.App.WebApi.Handlers
{
    public class DeepLoggingHandler : DelegatingHandler
    {
        private static readonly Lazy<ILog> Logger = new Lazy<ILog>(LogManager.GetLogger<DeepLoggingHandler>);

        private readonly IDeepLogger _deepLogger;

        public DeepLoggingHandler(IDeepLogger deepLogger)
        {
            _deepLogger = deepLogger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri.LocalPath.ToLower().Equals("/") ||
                request.RequestUri.LocalPath.ToLower().StartsWith("/help") ||
                request.RequestUri.LocalPath.ToLower().StartsWith("/docs"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            Guid correlationId = Guid.NewGuid();
            Logger.Value.Trace($"[{request.Method}]{request.RequestUri} started.");
            string requestContent = request.Content.ReadAsStringAsync().Result;
            DateTime startedOn = TrendyolApp.Instance.DateTimeProvider.Now;
            Task<HttpResponseMessage> responseAwaitingTask = base.SendAsync(request, cancellationToken);
            DateTime finishedOn = TrendyolApp.Instance.DateTimeProvider.Now;

            HttpResponseMessage response = responseAwaitingTask.Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;

            Log(correlationId, startedOn, finishedOn, request, requestContent, response, responseContent);
            Logger.Value.Trace($"[{request.Method}]{request.RequestUri} finished.");
            return responseAwaitingTask;
        }

        private void Log(Guid correlationId, DateTime startedOn, DateTime finishedOn, HttpRequestMessage request,
            string requestBody, HttpResponseMessage response,
            string responseBody)
        {
            string requestUrl = request.RequestUri.ToString();
            string httpMethod = request.Method.ToString();
            string requestHeaders = request.Headers.ToString();

            string responseCode = response.StatusCode.ToString();
            string responseHeaders = response.Content.Headers.ToString();

            Task.Run(() =>
            {
                try
                {
                    _deepLogger.Log(correlationId.ToString(), startedOn, finishedOn, requestUrl, httpMethod, requestHeaders, requestBody, responseCode, responseHeaders, responseBody);
                }
                catch (Exception ex)
                {
                    Logger.Value.Error($"There was a problem while deep logging request:[{httpMethod}]{requestUrl}.", ex);
                }
            });
        }
    }
}