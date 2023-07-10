using System.Net.Http.Json;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Models;
using Botticelli.Server.FrontNew.Settings;
using Botticelli.Server.Models.Responses;
using Flurl;
using Microsoft.Extensions.Options;

namespace Botticelli.Server.FrontNew.Clients;

public class SessionClient
{
    private readonly IOptionsMonitor<BackSettings> _backSettings;
    private readonly HttpClient _httpClient;
    private Session _session;

    public SessionClient(IOptionsMonitor<BackSettings> backSettings)
    {
        _httpClient = new HttpClient();
        _backSettings = backSettings;
    }

    public async Task<Error> RegisterUser(string email, string password)
    {
        var request = new UserRegisterPost()
        {
            Email = email,
            UserName = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                     "/auth/Register"),
                                                         request);

        if (!response.IsSuccessStatusCode)
        {
            return new Error
            {
                Code = 1,
                UserMessage = $"Error registering user: {response.ReasonPhrase}!"
            };
        }

        return new Error
        {
            Code = 0, 
            UserMessage = string.Empty
        };

    }

    public async Task<bool> HasUsersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<bool>(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                            "/auth/HasUsers"));


        return response;
    }
    
    public async Task<(Session session, Error error)> CreateSession(string login, string password)
    {
        var request = new UserLoginPost
        {
            Email = login,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync(Url.Combine(_backSettings.CurrentValue.BackUrl,
                                                                     "/auth/GetToken"),
                                                         request);

        var tokenResponse = await response.Content.ReadFromJsonAsync<GetTokenResponse>();

        if (!tokenResponse.IsSuccess)
            return new ValueTuple<Session, Error>(default,
                                                  new Error
                                                  {
                                                      Code = 1,
                                                      UserMessage = $"Login error!"
                                                  });

        _session = new Session
        {
            SessionId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            Token = tokenResponse.Token,
            Login = login
        };

        return (_session, new Error
        {
            Code = 0,
            UserMessage = "Success"
        });
    }

    public Session GetSession() => _session;
}