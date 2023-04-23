using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace Botticelli.Scheduler.Extensions;

public static class BotExtensions
{
    private static readonly IList<string> JobIds
            = new List<string>(1000);

    /// <summary>
    ///     Adds a new scheduled bot  for a job
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="reliability"></param>
    /// <param name="message"></param>
    /// <param name="jobName"></param>
    /// <param name="jobDescription"></param>
    /// <param name="schedule"></param>
    /// <param name="botOptions"></param>
    /// <param name="sp"></param>
    /// <returns></returns>
    public static BotOptionsBuilder<TBot> AddJob<TBot>(this BotOptionsBuilder<TBot> botOptions,
                                                       IServiceProvider sp,
                                                       ILogger logger,
                                                       Reliability reliability,
                                                       Message message,
                                                       string jobName,
                                                       string jobDescription,
                                                       Schedule schedule)
            where TBot : BotSettings, IBot<TBot>, new()
    {
        var jobId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        RecurringJob.AddOrUpdate(jobId,
                                 () => RunAction<TBot>(sp,
                                                       logger,
                                                       reliability,
                                                       message,
                                                       jobName,
                                                       jobDescription),
                                 schedule.Cron);

        JobIds.Add(jobId);

        return botOptions;
    }

    /// <summary>
    ///     Runs an action inside a job
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="reliability">Reliability parameters</param>
    /// <param name="message">Message top send</param>
    /// <param name="jobName">Job name</param>
    /// <param name="jobDescription">Job description</param>
    /// <param name="sp"></param>
    /// <returns></returns>
    private static Task RunAction<TBot>(IServiceProvider sp,
                                        ILogger logger,
                                        Reliability reliability,
                                        Message message,
                                        string jobName,
                                        string jobDescription)
            where TBot : BotSettings, IBot<TBot>, new()
    {
        var botAction = new BotAction<TBot>(sp,
                                            reliability,
                                            message,
                                            jobName,
                                            jobDescription,
                                            logger);

        return botAction.RunAsync(CancellationToken.None);
    }
}