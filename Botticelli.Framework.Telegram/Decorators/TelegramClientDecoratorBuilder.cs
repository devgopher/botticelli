using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Options;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Botticelli.Framework.Telegram.Decorators;

public class TelegramClientDecoratorBuilder
{
    private TelegramClientDecorator? _telegramClient;
    private IThrottler? _throttler;
    private HttpClient? _httpClient;
    private readonly BotOptionsBuilder<TelegramBotSettings> _optionsBuilder;
    private readonly IServiceCollection _services;
    private string? _token;

    public static TelegramClientDecoratorBuilder Instance(IServiceCollection services, BotOptionsBuilder<TelegramBotSettings> optionsBuilder) 
        => new(services, optionsBuilder);

    private TelegramClientDecoratorBuilder(IServiceCollection services, BotOptionsBuilder<TelegramBotSettings> optionsBuilder)
    {
        _services = services;
        _optionsBuilder = optionsBuilder;
    }

    public TelegramClientDecoratorBuilder AddHttpClient(HttpClient client)
    {
        _httpClient = client;

        return this;
    }

    public TelegramClientDecoratorBuilder AddThrottler(IThrottler throttler)
    {
        _throttler = throttler;
        
        return this;
    }

    public TelegramClientDecoratorBuilder AddToken(string token)
    {
        _token = token;

        return this;
    }
    
    public TelegramClientDecorator Build()
    {
        if (_token == default) throw new ArgumentNullException(nameof(_token));

        if (_telegramClient != null) return _telegramClient;
        
        if (_httpClient == default)
        {
            var factory = _services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

            _httpClient = factory.CreateClient(nameof(TelegramClientDecorator));
        }

        var botOptions = _optionsBuilder.Build();
        var clientOptions = new TelegramBotClientOptions(_token, botOptions.TelegramBaseUrl, botOptions.UseTestEnvironment ?? false);
        _telegramClient = new TelegramClientDecorator(clientOptions, _throttler, _httpClient);
        
        return _telegramClient;
    }
}