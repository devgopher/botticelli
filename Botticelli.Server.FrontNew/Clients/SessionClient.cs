using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Models;
using System.Net.Http.Json;

namespace Botticelli.Server.FrontNew.Clients
{
    public class SessionClient
    {
        private readonly List<Session> _sessions = new(100);
        private readonly HttpClient _httpClient;
        public SessionClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Session> CreateSession(string login, string password)
        {
            var request = new UserLoginPost
            {
                Email = login,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5050/login/GetToken", 
                                                             JsonContent.Create(request));
            
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
