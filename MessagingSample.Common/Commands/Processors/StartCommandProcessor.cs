using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Scheduler;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MessagingSample.Common.Commands.Processors;

public class StartCommandProcessor : CommandProcessor<StartCommand>
{
    public StartCommandProcessor(ILogger<StartCommandProcessor> logger,
        ICommandValidator<StartCommand> validator)
        : base(logger, validator)
    {
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
        var assemblyPath = Path.GetDirectoryName(typeof(StartCommandProcessor).Assembly.Location);
        JobManager.AddJob(Bots.FirstOrDefault(),
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
                Subject = "",
                ChatIds = new List<string> { chatId },
                Attachments = new List<IAttachment>
                {
                    new BinaryAttachment(Guid.NewGuid().ToString(),
                        "testpic.png",
                        MediaType.Image,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/testpic.png"), token)),
                    new BinaryAttachment(Guid.NewGuid().ToString(),
                        "voice.mp3",
                        MediaType.Voice,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/voice.mp3"), token)),
                    new BinaryAttachment(Guid.NewGuid().ToString(),
                        "video.mp4",
                        MediaType.Video,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/video.mp4"), token)),
                    new BinaryAttachment(Guid.NewGuid().ToString(),
                        "document.odt",
                        MediaType.Document,
                        string.Empty,
                        await File.ReadAllBytesAsync(Path.Combine(assemblyPath, "Media/document.odt"), token))
                }
            },
            new Schedule
            {
                Cron = "* * * * *"
            });
    }
}