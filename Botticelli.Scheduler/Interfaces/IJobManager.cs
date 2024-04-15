using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Scheduler.Interfaces;

public interface IJobManager
{
    string AddJob(IBot bot,
        Reliability reliability,
        Message message,
        Scheduler.Schedule schedule,
        Action<Message> preprocessFunc = default);

    void RemoveJob(string jobId);
    void RemoveAllJobs();

    Task SendWithReliability(IBot bot,
        SendMessageRequest request,
        Reliability reliability,
        CancellationToken token);
}