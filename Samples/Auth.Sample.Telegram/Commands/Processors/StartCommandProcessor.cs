using System.Reflection;
using Botticelli.Auth.Data;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Services;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;

namespace Auth.Sample.Telegram.Commands.Processors;

public class StartCommandProcessor<TReplyMarkup> : CommandProcessor<StartCommand> where TReplyMarkup : class
{
    private readonly SendOptionsBuilder<TReplyMarkup>? _options;
    private readonly IIdentifier<BotAuthCredentials, BotUserInfo> _userInfo;
    private readonly IManager<BotUserRoleInfo> _roleManager;

    public StartCommandProcessor(ILogger<StartCommandProcessor<TReplyMarkup>> logger,
                                 ICommandValidator<StartCommand> commandValidator,
                                 MetricsProcessor metricsProcessor,
                                 ILayoutSupplier<TReplyMarkup> layoutSupplier,
                                 ILayoutParser layoutParser,
                                 IValidator<Message> messageValidator,
                                 IIdentifier<BotAuthCredentials, BotUserInfo> userInfo,
                                 IManager<BotUserRoleInfo> roleManager)
            : base(logger,
                   commandValidator,
                   metricsProcessor,
                   messageValidator)
    {
        _userInfo = userInfo;
        _roleManager = roleManager;
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        var responseLayout = layoutParser.ParseFromFile(Path.Combine(location, "main_layout.json"));
        var responseMarkup = layoutSupplier.GetMarkup(responseLayout);

        _options = SendOptionsBuilder<TReplyMarkup>.CreateBuilder(responseMarkup);
    }

    protected override Task InnerProcessContact(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessLocation(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override async Task InnerProcess(Message message, CancellationToken token)
    {
        var user = await _userInfo.Identify(new BotAuthCredentials {UserId = message.ChatIds.First()});
        var role = (await _roleManager.Get()).FirstOrDefault(r => r.Id == user.User?.RoleId);
        
        var greetingMessageRequest = new SendMessageRequest
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = $"Bot started... Your role is: {role?.RoleName ?? DefaultRoles.Guest}"
            }
        };

        await Bot.SendMessageAsync(greetingMessageRequest, _options, token);
    }
}