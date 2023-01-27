using Botticelli.Talks.Exceptions;
using Botticelli.Talks.Settings;
using Flurl;
using Microsoft.Extensions.Options;

namespace Botticelli.Talks.OpenTts
{
    public class OpenTtsSpeaker : ISpeaker
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsMonitor<TtsSettings> _settings;

        public OpenTtsSpeaker(IHttpClientFactory httpClientFactory, 
                              IOptionsMonitor<TtsSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
        }

        public async Task<byte[]> Speak(string markedText, string lang, CancellationToken token)
        {
            var urlEncodedText = $"?voice={_settings.CurrentValue.DefaultVoice}:{lang}&text={Url.Encode(markedText)}&vocoder=high&denoiserStrength=0.03&cache=true";

            using (var client = _httpClientFactory.CreateClient())
            {
                var fullUrl = Url.Combine(_settings.CurrentValue.EngineConnection, urlEncodedText);
                var result = await client.GetAsync(fullUrl, token);
                
                if (!result.IsSuccessStatusCode) 
                    throw new BotticelliTalksException($"Can't get response from engine: {result.StatusCode}!");

                 var byteResult = await result.Content.ReadAsByteArrayAsync(token);
                return byteResult;
            }
        }
    }
}
