namespace Botticelli.Framework.Vk.Tests;

internal class TestHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
        => new(new SocketsHttpHandler());
}