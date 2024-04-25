using System.Reflection;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Scheduler;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using MessagingSample.Common.Layouts;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace MessagingSample.Common.Commands.Processors;

public class StartCommandProcessor : CommandProcessor<StartCommand>
{
    private readonly IJobManager _jobManager;
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;
    
    public StartCommandProcessor(ILogger<StartCommandProcessor> logger,
                                 ICommandValidator<StartCommand> validator,
                                 MetricsProcessor metricsProcessor,
                                 IJobManager jobManager,
                                 ITelegramLayoutSupplier layoutSupplier,
                                 ILayoutParser layoutParser)
        : base(logger, validator, metricsProcessor)
    {
        _jobManager = jobManager;

        var responseLayout = layoutParser.ParseFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location), "layout.json"));
        var responseMarkup = layoutSupplier.GetMarkup(responseLayout);
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(responseMarkup);
    }

    protected override async Task InnerProcessContact(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var chatId = message.ChatIds.FirstOrDefault();
        var greetingMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Bot started..."
            }
        };

        await Bot.SendMessageAsync(greetingMessageRequest, _options, token);

        var assemblyPath = Path.GetDirectoryName(typeof(StartCommandProcessor).Assembly.Location);
        _jobManager.AddJob(Bot,
            new Reliability
            {
                IsEnabled = false,
                Delay = TimeSpan.FromSeconds(3),
                IsExponential = true,
                MaxTries = 5
            },
            new Message
            {
                Body = "Now you see me!",
                ChatIds = [chatId],
                Contact = new Contact
                {
                    Phone = "+9003289384923842343243243",
                    Name = "Test",
                    Surname = "Botticelli"
                },
                Poll = new Poll
                {
                    Question = "To be or not to be?",
                    Variants = new []
                    {
                        "To be!",
                        "Not to be!"
                    },
                    CorrectAnswerId = 0,
                    IsAnonymous = false,
                    Type = Poll.PollType.Quiz
                },
                Attachments =
                [
                    new BinaryBaseAttachment(Guid.NewGuid().ToString(),
                        "testpic.png",
                        MediaType.Image,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/testpic.png"), token)),

                    new BinaryBaseAttachment(Guid.NewGuid().ToString(),
                        "voice.mp3",
                        MediaType.Voice,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/voice.mp3"), token)),

                    new BinaryBaseAttachment(Guid.NewGuid().ToString(),
                        "video.mp4",
                        MediaType.Video,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/video.mp4"), token)),

                    new BinaryBaseAttachment(Guid.NewGuid().ToString(),
                        "document.odt",
                        MediaType.Document,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/document.odt"), token))
                ]
            },
            new Schedule
            {
                Cron = "*/30 * * ? * * *"
            });
    }
}