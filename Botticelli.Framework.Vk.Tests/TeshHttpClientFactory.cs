using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Http.Logging;

namespace Botticelli.Framework.Vk.Tests
{
    internal class TestHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
            => new(new SocketsHttpHandler());
    }
}
