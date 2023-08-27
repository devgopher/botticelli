using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Vk;
using Botticelli.Interfaces;
using Botticelli.Scheduler;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Requests.Abstractions;
using VkMessagingSample.Commands;

namespace VkMessagingSample;

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
  
       // Thread.Sleep(20000);


        JobManager.AddJob((VkBot) Bots.FirstOrDefault(),
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
                         ChatIds = new() {chatId},
                         Attachments = new List<IAttachment>
                         {
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "testpic.png",
                                                           MediaType.Image,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/testpic.png", token)),
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "voice.mp3",
                                                           MediaType.Voice,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/voice.mp3", token)),
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "video.mp4",
                                                           MediaType.Video,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/video.mp4", token))
                         }
                     },
                     new Schedule
                     {
                         Cron = "* * * * *"
                     });

    }
}