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
            return await _innerClient?.SendRequest(request, cancellationToken)!;
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine(ex);

            throw;
        }
    }

    [Obsolete("Use SendRequest")]
    public Task<TResponse> MakeRequest<TResponse>(IRequest<TResponse> request,
                                                  CancellationToken cancellationToken = new())
    {
        return SendRequest(request, cancellationToken);
    }

    [Obsolete("Use SendRequest")]
    public async Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request,
                                                             CancellationToken cancellationToken = new())
    {
        return await SendRequest(request, cancellationToken);
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