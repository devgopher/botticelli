using System.Text.Encodings.Web;
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

        public async Task<byte[]> Speak(string markedText, string language, CancellationToken token)
        {
            var urlEncodedText = Url.Encode($"voice=espeak:en&text={markedText}&vocoder=high&denoiserStrength=0.03&cache=true", encodeSpaceAsPlus: true);

            using (var client = _httpClientFactory.CreateClient())
            {
                var result = await client.GetAsync(Url.Combine(_settings.CurrentValue.EngineConnection, urlEncodedText), token);
                
                if (!result.IsSuccessStatusCode) 
                    throw new BotticelliTalksException($"Can't get response from engine: {result.StatusCode}!");

                await using var byteResult = await result.Content.ReadAsStreamAsync(token);

                var bytes = new Memory<byte>();
                var readCount = await byteResult.ReadAsync(bytes, token);
                return readCount > 0 ? bytes.ToArray() : Array.Empty<byte>();
            }
        }
    }
}
