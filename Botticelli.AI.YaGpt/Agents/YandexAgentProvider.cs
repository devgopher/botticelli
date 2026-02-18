using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Botticelli.AI.Agents;
using Botticelli.AI.Agents.Models;
using Botticelli.AI.Exceptions;
using Botticelli.AI.Message;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.YaGpt.Agents;

/// <summary>
/// Agent provider that communicates with the Yandex GPT (YaGpt) API.
/// Sends user messages as assistant requests and maps assistant responses back to bus messages.
/// </summary>
public class YandexAgentProvider : BasicAgentProvider
{
    /// <inheritdoc />
    public override string AiName => "YaGpt";

    private const string ContentType = "application/json";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    private readonly IOptions<AgentSettings> _settings;
    private readonly HttpClient _client;

    /// <summary>
    /// Creates a new Yandex GPT agent provider with the given settings and dependencies.
    /// </summary>
    /// <param name="settings">Agent configuration (URL, model, temperature, max tokens).</param>
    /// <param name="client">HTTP client for API calls.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="bus">Bus client for sending responses.</param>
    /// <param name="messageValidator">Validator for incoming AI messages.</param>
    public YandexAgentProvider(IOptions<AgentSettings> settings,
        HttpClient client,
        ILogger logger,
        IBusClient bus,
        IValidator<AiMessage> messageValidator) : base(client, logger, bus, messageValidator)
    {
        _settings = settings;
        _client = client;
        _client.BaseAddress = new(settings.Value.Url);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
    }

    /// <inheritdoc />
    protected override async Task<AssistantResponse> GetAgentResponse(AiMessage message, CancellationToken token)
    {
        try
        {
            var json = JsonSerializer.Serialize(new AssistantRequest
            {
                Model = _settings.Value.Model,
                Input = message.Body ?? string.Empty,
                Temperature = _settings.Value.Temperature,
                MaxOutputTokens = _settings.Value.MaxOutputTokens
            }, _jsonOptions);
            
            var content = new StringContent(json, Encoding.UTF8, ContentType);

            var response = await _client.PostAsync(_settings.Value.Url, content, token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<AssistantResponse>(_jsonOptions, token)
                .ConfigureAwait(false);

            return result ?? throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (HttpRequestException ex)
        {
            throw new AiException("HTTP request failed: {ex}", ex);
        }
        catch (JsonException ex)
        {
            throw new AiException("JSON serialization error: {ex}", ex);
        }
    }
}