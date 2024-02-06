using Botticelli.Server.Settings;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;

namespace Botticelli.Server.Services.Auth;

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
        _logger.LogInformation($"Sending a password message to : {email}");

        var message = Email.From(_serverSettings.ServerEmail, "BotticelliBots Admin Service")
            .To(email)
            .Subject("BotticelliBots user credentials")
            .Body($"Your login/password: {email} / {password}");

        if (ct.CanBeCanceled && ct.IsCancellationRequested)
            return;

        var sendResult = await _fluentEmail.SendAsync(message, ct);

        if (!sendResult.Successful)
        {
            _logger.LogError($"Sending a password message to : {email} error", sendResult.ErrorMessages);
            throw new InvalidOperationException($"Sending mail errors:  {string.Join(',', sendResult.ErrorMessages)}");
        }

        _logger.LogInformation($"Sending a password message to : {email} - OK");
    }
}