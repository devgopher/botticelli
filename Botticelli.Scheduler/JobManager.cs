using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Hangfire;

namespace Botticelli.Scheduler;

public static class JobManager
{
    private static readonly List<string> JobIds = new(5);

    public static string AddJob(IBot bot,
        Reliability reliability,
        Message message,
        Schedule schedule,
        Action<Message> preprocessFunc = default)
    {
        var jobId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var request = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = message
        };

        preprocessFunc?.Invoke(request.Message);

        if (!reliability.IsEnabled)
        {
            RecurringJob.AddOrUpdate(jobId,
                () => bot.SendMessageAsync(request, CancellationToken.None),
                schedule.Cron);
            JobIds.Add(jobId);

            return jobId;
        }

        RecurringJob.AddOrUpdate(jobId,
            () => SendWithReliability(bot,
                request,
                reliability,
                CancellationToken.None),
            schedule.Cron);
        JobIds.Add(jobId);

        return jobId;
    }

    public static void RemoveJob(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
        JobIds.Remove(jobId);
    }
    public static void RemoveAllJobs()
    {
        foreach (var jobId in JobIds)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        JobIds.Clear();
    }

    public static async Task SendWithReliability(IBot bot,
        SendMessageRequest request,
        Reliability reliability,
        CancellationToken token)
        => throw new NotImplementedException();
}