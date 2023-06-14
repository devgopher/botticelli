using System.Net.Http.Json;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Models;
using Botticelli.Server.FrontNew.Settings;
using Botticelli.Server.Models.Responses;
using Flurl;
using Microsoft.Extensions.Options;

namespace Botticelli.Server.FrontNew.Clients
{
    public class SessionClient
    {
        private Session _session;
        private readonly IOptionsMonitor<BackSettings> _backSettings;
        private readonly HttpClient _httpClient;

        public SessionClient(IOptionsMonitor<BackSettings> backSettings)
        {
            _httpClient = new HttpClient();
            _backSettings = backSettings;
        }

        public async Task<(Session session, Error error)> CreateSession(string login, string password)
        {
            var request = new UserLoginPost
            {
                Email = login,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                         "/login/GetToken"), request);

            var tokenResponse = await response.Content.ReadFromJsonAsync<GetTokenResponse>();

            if (!tokenResponse.IsSuccess)
                return new ValueTuple<Session, Error>(default,
                                                      new Error
                                                      {
                                                          UserMessage = "Login error!"
                                                      });

            _session = new Session
            {
                SessionId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Token = tokenResponse.Token,
                Login = login
            };

            return (_session, new Error
            {
                UserMessage = "Success"
            });
        }

        public Session GetSession() => _session;
    }
}
