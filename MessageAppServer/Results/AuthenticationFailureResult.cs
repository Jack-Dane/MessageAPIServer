using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace MessageAppServer.Results
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage reqeust)
        {
            ReasonPhrase = reasonPhrase;
            Request = reqeust;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }
}
