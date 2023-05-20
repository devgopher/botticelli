using Botticelli.Server.FrontNew.Clients;
using System.Security.Authentication;

namespace Botticelli.Server.FrontNew.Handler
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly SessionClient _sessionClient;

        public AuthDelegatingHandler(SessionClient sessionClient) => _sessionClient = sessionClient;

        protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sid = request.Headers.GetValues("sessionId").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sid))
                await base.SendAsync(request, cancellationToken);

            var session = _sessionClient.GetSessionById(sid);

            if (session == default) 
                throw new AuthenticationException($"Can't find session {sid}!");

            request.Headers.Remove("sessionId");
            request.Headers.Add("token", session.Token);

            var response = await base.SendAsync(request, cancellationToken);
            
            return response;
        }
    }
}
