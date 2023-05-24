using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Models;
using Botticelli.Server.FrontNew.Settings;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

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

        public async Task<Session> CreateSession(string login, string password)
        {
            Console.WriteLine($"SS: {login} {password}" );
            var request = new UserLoginPost
            {
                Email = login,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                         "/login/GetToken"), request);
            
            var token =  await response.Content.ReadAsStringAsync();
            _session = new Session
            {
                SessionId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Token = token,
                Login = login
            };

            return _session;
        }

        public Session GetSession() => _session;
    }
}
