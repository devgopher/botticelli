using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Hangfire;

namespace Botticelli.Scheduler;

public static class JobManager
{
    public static string AddJob<TBot>(IBot<TBot> bot,
                                      Reliability reliability,
                                      Message message,
                                      Schedule schedule)
            where TBot : IBot<TBot>
    {
        var jobId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var request = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = message
        };

        if (!reliability.IsEnabled)
        {
            RecurringJob.AddOrUpdate(jobId,
                                     () => bot.SendMessageAsync(request, CancellationToken.None),
                                     schedule.Cron);

            return jobId;
        }

        RecurringJob.AddOrUpdate(jobId,
                                 () => SendWithReliability(bot,
                                                           request,
                                                           reliability,
                                                           CancellationToken.None),
                                 schedule.Cron);

        return jobId;
    }

    public static void RemoveJob(string jobId) => RecurringJob.RemoveIfExists(jobId);

    public static async Task SendWithReliability<TBot>(IBot<TBot> bot,
                                                       SendMessageRequest request,
                                                       Reliability reliability,
                                                       CancellationToken token)
            where TBot : IBot<TBot> =>
            throw new NotImplementedException();
    //try
    //{
    //    int t = 0;
    //    while (reliability.IsEnabled && t < reliability.MaxTries)
    //    {
    //        var response = await bot.SendMessageAsync(request, token);
    //        if (response.MessageSentStatus == MessageSentStatus.Ok) return;
    //        if (reliability.IsExponential)
    //        {
    //            Thread.Sleep((int) (t * Math.Exp(reliability.Delay.TotalMilliseconds)));
    //            continue;
    //        }
    //        Thread.Sleep((int)reliability.Delay.TotalMilliseconds);
    //        ++t;
    //    }
    //}
    //catch (Exception ex)
    //{
    //}
}