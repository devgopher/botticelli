using System.Net.Http.Headers;
using Botticelli.Server.FrontNew.Clients;

namespace Botticelli.Server.FrontNew.Handler;

public class AuthDelegatingHandler(SessionClient sessionClient) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var session = sessionClient.GetSession();

        if (session == null) throw new UnauthorizedAccessException("Can't find session!");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}