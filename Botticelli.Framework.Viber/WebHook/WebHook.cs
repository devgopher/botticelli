using System.Net;
using System.Net.Mime;
using Botticelli.Framework.Viber.Messages.Callbacks;
using Botticelli.Framework.Viber.Options;
using Botticelli.Serialization;
using Botticelli.Shared.Utils;
using Microsoft.Extensions.Logging;
using Viber.Api.Settings;

namespace Botticelli.Framework.Viber.WebHook
{
    public class WebHook
    {
        private readonly ISerializerFactory _serializerFactory;
        private readonly ILogger<WebHook> _logger;
        private readonly ViberBotSettings _settings;
        private readonly HttpListener _listener;


        public WebHook(ISerializerFactory serializerFactory,
                       ILogger<WebHook> logger,
                       ViberBotSettings settings)
        {
            _serializerFactory = serializerFactory;
            _logger = logger;
            _settings = settings;
            _listener = new HttpListener();
            _listener.Prefixes.Add("https://localhost:443");
        }

        public async Task Start(CancellationToken token)
        {
            _listener.Start();

            var listenerTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var context = await _listener.GetContextAsync();
                        using var response = context.Response;

                        response.StatusCode = (int) HttpStatusCode.OK;
                        response.StatusDescription = "OK";

                        if (context.Request == default)
                        {
                            _logger.LogDebug($"Empty request!");

                            continue;
                        }

                        if (!context.Request
                                    .ContentType
                                    .ToLowerInvariant()
                                    .Contains("json"))
                        {
                            _logger.LogDebug($"Wrong content type: {context.Request.ContentType}");

                            continue;
                        }

                        var json = StreamUtils.FromStream(context.Request.InputStream);

                        var generic = _serializerFactory.GetSerializer<BasicCallback>()
                                                        .Deserialize(json);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                }

                _listener.Stop();
                return;
            }, token);


        }
    }
}
