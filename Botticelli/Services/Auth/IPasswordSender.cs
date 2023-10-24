using Botticelli.Server.Settings;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using Flurl;

namespace Botticelli.Server.Services.Auth
{
    public interface IPasswordSender
    {
        public Task SendPassword(string email, string password, CancellationToken ct);
    }

    public class PasswordSender :IPasswordSender
    {
        private readonly ISender _fluentEmail;
        private readonly ServerSettings _serverSettings;

        public PasswordSender(ISender fluentEmail, ServerSettings serverSettings)
        {
            _fluentEmail = fluentEmail;
            _serverSettings = serverSettings;
        }

        public async Task SendPassword(string email, string password, CancellationToken ct)
        {
            var message = Email.From(_serverSettings.ServerEmail, "BotticelliBots Admin Service")
                .To(email)
                .Subject("BotticelliBots user credentials")
                .Body($"Your login/password: {email}/{password}");

            if (ct.CanBeCanceled && ct.IsCancellationRequested)
                return;
            
            var sendResult = await _fluentEmail.SendAsync(message, ct);

            if (!sendResult.Successful)
                throw new InvalidOperationException($"Sending mail errors:  {string.Join(',', sendResult.ErrorMessages)}");
        }
    }
}
