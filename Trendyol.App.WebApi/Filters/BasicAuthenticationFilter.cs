using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Trendyol.App.Authentication;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.WebApi.Models;

namespace Trendyol.App.WebApi.Filters
{
    internal class BasicAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IAuthenticationChecker _checker;
        private readonly IUserStore _userStore;

        public BasicAuthenticationFilter(IAuthenticationChecker checker, IUserStore userStore)
        {
            _checker = checker;
            _userStore = userStore;
        }

        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
            if (userNameAndPasword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            IPrincipal principal = null;
            if (_checker.Check(userName, password))
            {
                IUser user = _userStore.GetUserByUsername(userName);
                if (user == null)
                {
                    return;
                }

                GenericIdentity identity = new GenericIdentity(user.Username);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                if (user.Roles != null)
                {
                    foreach (string userRole in user.Roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, userRole));
                    }
                }

                principal = new ClaimsPrincipal(identity);
            }

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                return;
            }

            context.Principal = principal;
        }


        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }


        private Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            authorizationParameter = Encoding.Default.GetString(Convert.FromBase64String(authorizationParameter));

            string[] tokens = authorizationParameter.Split(':');
            return tokens.Length != 2
                       ? null
                       : new Tuple<string, string>(tokens[0], tokens[1]);
        }


        private class AddChallengeOnUnauthorizedResult : IHttpActionResult
        {
            public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
            {
                Challenge = challenge;
                InnerResult = innerResult;
            }

            public AuthenticationHeaderValue Challenge { get; private set; }

            public IHttpActionResult InnerResult { get; private set; }

            public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (response.Headers.WwwAuthenticate.All(h => h.Scheme != Challenge.Scheme))
                    {
                        response.Headers.WwwAuthenticate.Add(Challenge);


                        ErrorResponse errorResponse = new ErrorResponse();
                        errorResponse.AddErrorMessage("Unauthorized");

                        response = response.RequestMessage.CreateResponse(HttpStatusCode.Unauthorized, errorResponse);
                    }
                }

                return response;
            }
        }

        private class AuthenticationFailureResult : IHttpActionResult
        {
            public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
            {
                ReasonPhrase = reasonPhrase;
                Request = request;
            }

            private string ReasonPhrase { get; set; }

            private HttpRequestMessage Request { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute());
            }

            private HttpResponseMessage Execute()
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.AddErrorMessage(ReasonPhrase);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized, errorResponse);
                return response;
            }
        }
    }
}