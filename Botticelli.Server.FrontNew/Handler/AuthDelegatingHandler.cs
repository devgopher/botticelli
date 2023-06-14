using System.Net.Http.Headers;
using Botticelli.Server.FrontNew.Clients;
using System.Security.Authentication;
using System.Net.Http;

namespace Botticelli.Server.FrontNew.Handler
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly SessionClient _sessionClient;

        public AuthDelegatingHandler(SessionClient sessionClient) => _sessionClient = sessionClient;

        protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = _sessionClient.GetSession();

            Console.WriteLine($"Auth delegating got session: {session?.Token}");
            
            if (session == default) 
                throw new AuthenticationException($"Can't find session!");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);

            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine($"Resp: {await response.Content.ReadAsStringAsync()}");
            return response;
        }
    }
}
