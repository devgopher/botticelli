using System.Net.Http.Headers;
using Botticelli.Server.FrontNew.Clients;
using System.Security.Authentication;

namespace Botticelli.Server.FrontNew.Handler
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly SessionClient _sessionClient;

        public AuthDelegatingHandler(SessionClient sessionClient)
        {
            Console.WriteLine("!!!!!");
            _sessionClient = sessionClient;

        }

        protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("SSSS");
            var session = _sessionClient.GetSession();
            Console.WriteLine($"Auth delegating got session: {session?.SessionId}");

            if (session == default) 
                throw new AuthenticationException($"Can't find session!");

            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {session.Token}");

            var response = await base.SendAsync(request, cancellationToken);
            
            return response;
        }
    }
}
