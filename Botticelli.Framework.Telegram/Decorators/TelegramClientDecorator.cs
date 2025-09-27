using Botticelli.Framework.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace Botticelli.Framework.Telegram.Decorators;

/// <summary>
///     A Telegram.Bot decorator with auto-retry on 429 error
/// </summary>
public class TelegramClientDecorator : ITelegramBotClient
{
    private readonly HttpClient? _httpClient;
    private TelegramBotClient? _innerClient;
    private TelegramBotClientOptions _options;

    #region Limits
    private const int MessagesInSecond = 30;
    private DateTime? _prev;
    private long? _sentMessageCount = 0;
    #endregion Limits

    internal TelegramClientDecorator(TelegramBotClientOptions options,
                                     HttpClient? httpClient = null)
    {
        _options = options;
        _httpClient = httpClient;
        _innerClient = !string.IsNullOrWhiteSpace(options.Token) ? new TelegramBotClient(options, httpClient) : null;
    }


    public async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request,
                                                        CancellationToken cancellationToken = new())
    {
        try
        {
            _prev ??= DateTime.UtcNow;
            _sentMessageCount ??= 0;
            var deltaT = (DateTime.UtcNow - _prev).Value;
            
            if (_sentMessageCount < MessagesInSecond && deltaT < TimeSpan.FromSeconds(1.0))
                return await _innerClient?.SendRequest(request, cancellationToken)!;
            else
            {
                await Task.Delay(deltaT, cancellationToken);

                _sentMessageCount = 0;
                _prev = DateTime.UtcNow;
                
                return await _innerClient?.SendRequest(request, cancellationToken);
            }
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine(ex);

            throw;
        }
        finally
        {
            ++_sentMessageCount;
        }
    }

    public Task<bool> TestApi(CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }


    public async Task DownloadFile(string filePath,
                                   Stream destination,
                                   CancellationToken cancellationToken = new())
    {
        try
        {
            if (_innerClient != null) await _innerClient?.DownloadFile(filePath, destination, cancellationToken)!;
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine(ex);

            throw;
        }
    }

    public Task DownloadFile(TGFile file, Stream destination, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public bool LocalBotServer { get; set; } = false;
    public long BotId { get; set; } = -1;
    public TimeSpan Timeout { get; set; }
    public IExceptionParser ExceptionsParser { get; set; } = new DefaultExceptionParser();
    public event AsyncEventHandler<ApiRequestEventArgs>? OnMakingApiRequest;
    public event AsyncEventHandler<ApiResponseEventArgs>? OnApiResponseReceived;


    public void ChangeBotToken(string token)
    {
        _options = new TelegramBotClientOptions(token, _options.BaseUrl, _options.UseTestEnvironment);
        _innerClient = new TelegramBotClient(_options, _httpClient);
    }
}