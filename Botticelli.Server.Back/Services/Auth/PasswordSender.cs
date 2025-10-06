using Botticelli.Server.Back.Extensions;
using Botticelli.Server.Back.Settings;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;

namespace Botticelli.Server.Back.Services.Auth;

public class PasswordSender : IPasswordSender
{
    private readonly ISender _fluentEmail;
    private readonly ILogger<PasswordSender> _logger;
    private readonly ServerSettings _serverSettings;

    public PasswordSender(ISender fluentEmail, ServerSettings serverSettings, ILogger<PasswordSender> logger)
    {
        _fluentEmail = fluentEmail;
        _serverSettings = serverSettings;
        _logger = logger;
    }

    public async Task SendPassword(string email, string password, CancellationToken ct)
    {
        _logger.LogInformation("Sending a password message to : {Email}", email);

        if (EnvironmentExtensions.IsDevelopment()) _logger.LogInformation("!!! Email : {Email} password: {Password} !!! ONLY FOR DEVELOPMENT PURPOSES !!!", email, password);

        var message = Email.From(_serverSettings.ServerEmail, "BotticelliBots Admin Service")
                           .To(email)
                           .Subject("BotticelliBots user credentials")
                           .Body($"Your login/password: {email} / {password}");

        if (ct is {CanBeCanceled: true, IsCancellationRequested: true}) return;

        var sendResult = await _fluentEmail.SendAsync(message, ct);

        if (!sendResult.Successful)
        {
            _logger.LogError("Sending a password message to : {Email} error error mssage : {ErrMessage}", email, sendResult.ErrorMessages);

            throw new InvalidOperationException($"Sending mail errors:  {string.Join(',', sendResult.ErrorMessages)}");
        }

        _logger.LogInformation("Sending a password message to : {Email} - OK", email);
    }
}