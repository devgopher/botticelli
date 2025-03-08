using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Services;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;

namespace Auth.Sample.Telegram.Commands.Processors;

/// <summary>
///     Registers a bot user
/// </summary>
/// <param name="userManager"></param>
/// <param name="roleManager"></param>
/// <param name="logger"></param>
/// <param name="commandValidator"></param>
/// <param name="metricsProcessor"></param>
/// <param name="messageValidator"></param>
/// <typeparam name="TReplyMarkup"></typeparam>
public class RegisterCommandProcessor<TReplyMarkup>(
    IManager<BotUserInfo> userManager,
    IManager<BotUserRoleInfo> roleManager,
    ILogger<InfoCommandProcessor<TReplyMarkup>> logger,
    ICommandValidator<RegisterCommand> commandValidator,
    MetricsProcessor metricsProcessor,
    IValidator<Message> messageValidator)
    : CommandProcessor<RegisterCommand>(logger,
        commandValidator,
        metricsProcessor,
        messageValidator)
    where TReplyMarkup : class
{
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
        var chatId = message.ChatIds.Single();
        var user = (await userManager.Get()).FirstOrDefault(u => u.UserId == chatId);

        if (user != null)
        {
            var role = (await roleManager.Get()).FirstOrDefault(r => r.Id == user.RoleId);

            if (role != null)
            {
                var alreadyRegisteredRequest = new SendMessageRequest
                {
                    Message = new Message
                    {
                        Uid = Guid.NewGuid().ToString(),
                        ChatIds = message.ChatIds,
                        Body = $"You're already registered. Your role is: {role.RoleName}!"
                    }
                };

                await Bot.SendMessageAsync(alreadyRegisteredRequest, token);

                return;
            }

            role = await GetUserRole();
            user.RoleId = role!.Id;

            await userManager.Update(user);
        }
        else
        {
            var role = await GetUserRole();

            user = new BotUserInfo
            {
                UserId = chatId,
                UserName = string.Empty,
                RoleId = role!.Id
            };

            await userManager.Add(user);
        }

        var registeredRequest = new SendMessageRequest
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "You're successfully registered now.\nEnjoy!"
            }
        };

        await Bot.SendMessageAsync(registeredRequest, token)!;
    }

    private async Task<BotUserRoleInfo?> GetUserRole()
    {
        return (await roleManager.Get()).FirstOrDefault(r => r.RoleName == DefaultRoles.User);
    }
}