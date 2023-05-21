using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Models;
using System.Net.Http.Json;
using Botticelli.Server.FrontNew.Settings;
using Flurl;
using Microsoft.Extensions.Options;

namespace Botticelli.Server.FrontNew.Clients
{
    public class SessionClient
    {
        private readonly IList<Session> _sessions = new List<Session>(100);
        private readonly IOptionsMonitor<BackSettings> _backSettings;
        private readonly HttpClient _httpClient;
        public SessionClient(HttpClient httpClient, IOptionsMonitor<BackSettings> backSettings)
        {
            _httpClient = httpClient;
            _backSettings = backSettings;
        }

        public async Task<Session> CreateSession(string login, string password)
        {
            var request = new UserLoginPost
            {
                Email = login,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                         "/login/GetToken"), request);
            
            var token =  await response.Content.ReadAsStringAsync();
            var session = new Session
            {
                SessionId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Token = token,
                Login = login
            };

            _sessions.Add(session);

            return session;
        }

        public Session GetSessionByLogin(string login)
            => _sessions.FirstOrDefault(s => s.Login == login);

        public Session GetSessionById(string sid)
            => _sessions.FirstOrDefault(s => s.SessionId == sid);
    }
}
