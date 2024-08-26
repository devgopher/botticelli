using System.Net;
using Polly;
using Polly.Retry;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;

namespace Botticelli.Framework.Telegram.Decorators;

/// <summary>
/// A Telegram.Bot decorator with auto-retry on 429 error
/// </summary>
public class TelegramClientDecorator : ITelegramBotClient
{
    private readonly TelegramBotClient _bot;
    private readonly RetryPolicy _policy = Policy
        .Handle<ApiRequestException>(e => e.ErrorCode == (int)HttpStatusCode.TooManyRequests)
        .WaitAndRetry(10, (i , _) => TimeSpan.FromMilliseconds(1000 * Math.Exp(i)));
    
    public TelegramClientDecorator(string token,
        HttpClient? httpClient = null) =>
        _bot = new TelegramBotClient(token, httpClient);

    public async Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            return await _policy.Execute(() => _bot.MakeRequestAsync(request, cancellationToken)).ConfigureAwait(false);
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Task<bool> TestApiAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public async Task DownloadFileAsync(string filePath, Stream destination,
        CancellationToken cancellationToken = new())
    {
        try
        {
            await _policy.Execute(() => _bot.DownloadFileAsync(filePath, destination, cancellationToken)).ConfigureAwait(false);
        }
        catch (ApiRequestException ex)
        {
            
            Console.WriteLine(ex);
            throw;
        }
    }

    public bool LocalBotServer { get; }
    public long? BotId { get; }
    public TimeSpan Timeout { get; set; }
    public IExceptionParser ExceptionsParser { get; set; }
    public event AsyncEventHandler<ApiRequestEventArgs> OnMakingApiRequest;
    public event AsyncEventHandler<ApiResponseEventArgs> OnApiResponseReceived;
}