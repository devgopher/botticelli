using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Telegram.Decorators;

public class TelegramClientDecoratorBuilder
{
    private TelegramClientDecorator _telegramClient;
    private IThrottler? _throttler;
    private HttpClient? _httpClient;
    private IServiceCollection _services;
    private string? _token;
    
    public static TelegramClientDecoratorBuilder Instance(IServiceCollection services) => new(services);

    private TelegramClientDecoratorBuilder(IServiceCollection services) => _services = services;
    
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
        
        _telegramClient = new TelegramClientDecorator(token: _token, _throttler, _httpClient);
        
        return _telegramClient;
    }
}