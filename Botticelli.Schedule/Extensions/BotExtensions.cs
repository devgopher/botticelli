using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Hangfire;

namespace Botticelli.Scheduler.Extensions;

public static class BotExtensions
{
    private static readonly IList<string> JobIds
            = new List<string>(1000);

    /// <summary>
    ///     Adds a new scheduled bot  for a job
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="reliability"></param>
    /// <param name="message"></param>
    /// <param name="jobName"></param>
    /// <param name="jobDescription"></param>
    /// <param name="schedule"></param>
    /// <returns></returns>
    public static IBot AddJob(this IBot bot,
                              Reliability reliability,
                              Message message,
                              string jobName,
                              string jobDescription,
                              Schedule schedule)
    {
        var jobId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        RecurringJob.AddOrUpdate(jobId,
                                 () => RunAction(bot,
                                                 reliability,
                                                 message,
                                                 jobName,
                                                 jobDescription),
                                 schedule.Cron);

        JobIds.Add(jobId);

        return bot;
    }

    /// <summary>
    /// Runs an action inside a job
    /// </summary>
    /// <param name="bot">Bot</param>
    /// <param name="reliability">Reliability parameters</param>
    /// <param name="message">Message top send</param>
    /// <param name="jobName">Job name</param>
    /// <param name="jobDescription">Job description</param>
    /// <returns></returns>
    private static Task RunAction(IBot bot,
                                  Reliability reliability,
                                  Message message,
                                  string jobName,
                                  string jobDescription)
    {
        var botAction = new BotAction(reliability,
                                      message,
                                      bot,
                                      jobName,
                                      jobDescription);

        return botAction.RunAsync(CancellationToken.None);
    }
}