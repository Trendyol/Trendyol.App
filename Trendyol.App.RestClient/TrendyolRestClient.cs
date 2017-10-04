using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Trendyol.App.RestClient.Decorators;
using Trendyol.App.RestClient.Serialization;
using IRestRequest = RestSharp.IRestRequest;
using IRestResponse = RestSharp.IRestResponse;
using RestRequestAsyncHandle = RestSharp.RestRequestAsyncHandle;

namespace Trendyol.App.RestClient
{
    public class TrendyolRestClient : RestSharp.RestClient
    {
        protected RestSharp.RestClient DecoratedClient;

        public TrendyolRestClient(string baseUrl)
        {
            InitializeDefaults(baseUrl);
        }

        public TrendyolRestClient(string baseUrl, string clientId, string clientSecret, string tokenUrl, string scope)
        {
            InitializeDefaults(baseUrl);

            DecoratedClient = new OAuth2Decorator(DecoratedClient, clientId, clientSecret, tokenUrl, scope);
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            return DecoratedClient.Execute(request);
        }

        public override RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            return DecoratedClient.ExecuteAsync(request, callback);
        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            return DecoratedClient.Execute<T>(request);
        }

        public override RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            return DecoratedClient.ExecuteAsync(request, callback);
        }

        public override RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return DecoratedClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public override RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return DecoratedClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public override RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return DecoratedClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public override RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return DecoratedClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public override Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            return DecoratedClient.ExecuteGetTaskAsync(request);
        }

        public override Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecuteGetTaskAsync(request, token);
        }

        public override Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request)
        {
            return DecoratedClient.ExecuteGetTaskAsync<T>(request);
        }

        public override Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecuteGetTaskAsync<T>(request, token);
        }

        public override Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request)
        {
            return DecoratedClient.ExecutePostTaskAsync(request);
        }

        public override Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecutePostTaskAsync(request, token);
        }

        public override Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request)
        {
            return DecoratedClient.ExecutePostTaskAsync<T>(request);
        }

        public override Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecutePostTaskAsync<T>(request, token);
        }

        public override Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            return DecoratedClient.ExecuteTaskAsync(request);
        }

        public override Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecuteTaskAsync(request, token);
        }

        public override Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            return DecoratedClient.ExecuteTaskAsync<T>(request);
        }

        public override Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            return DecoratedClient.ExecuteTaskAsync<T>(request, token);
        }

        private void InitializeDefaults(string baseUrl)
        {
            DecoratedClient = new RestSharp.RestClient(baseUrl);
            DecoratedClient.AddHandler("application/json", new CustomJsonSerializer());
            DecoratedClient.AddDefaultHeader("Connection", "close");
        }
    }
}