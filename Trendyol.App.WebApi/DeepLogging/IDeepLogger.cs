using System;

namespace Trendyol.App.WebApi.DeepLogging
{
    public interface IDeepLogger
    {
        void Log(string correlationId, DateTime startedOn, DateTime finishedOn, string requestUrl, string requestMethod, string requestHeaders, string requestContent, string responseCode, string responseHeaders, string responseContent);
    }
}