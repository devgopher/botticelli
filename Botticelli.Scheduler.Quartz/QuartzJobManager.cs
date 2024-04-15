using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Botticelli.Interfaces;
using Botticelli.Scheduler;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Quartz;

namespace Botticelli.Schedule.Quartz;

public class QuartzJobManager : IJobManager, IDisposable
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly CancellationTokenSource _tokenSource = CancellationTokenSource.CreateLinkedTokenSource();
    private readonly List<TriggerKey> _triggerKeys = new(5);
    private IScheduler _scheduler;

    public QuartzJobManager(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public void Dispose()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }

    public string AddJob(IBot bot,
        Reliability reliability,
        Message message,
        Scheduler.Schedule schedule,
        Action<Message> preprocessFunc = default)
    {
        _scheduler ??= _schedulerFactory.GetScheduler().Result;

        if (!CronExpression.IsValidExpression(schedule.Cron))
            throw new InvalidDataException($"Cron {schedule?.Cron ?? "null"} is invalid!");

        var jobId = GetJobId();

        var request = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = message
        };

        preprocessFunc?.Invoke(request.Message);

        var job = !reliability.IsEnabled
            ? JobBuilder.Create<SendMessageJob>()
                .WithIdentity(jobId, "sendMessageJobGroup")
                .Build()
            : JobBuilder.Create<ReliableSendMessageJob>()
                .WithIdentity(jobId, "reliableSendMessageJobGroup")
                .Build();

        var triggerId = GetTriggerIdentity();

        var trigger = TriggerBuilder.Create()
            .ForJob(job)
            .WithCronSchedule(schedule.Cron)
            .WithIdentity(triggerId)
            .StartNow()
            .Build();

        var scheduleTask = _scheduler.ScheduleJob(job, trigger, _tokenSource.Token);
        scheduleTask.Start();

        _triggerKeys.Add(trigger.Key);

        return triggerId;
    }

    public void RemoveJob(string triggerId)
    {
        var existingKey = _triggerKeys.FirstOrDefault(k => k.Name == triggerId);
        if (existingKey is null)
            return;

        _scheduler.UnscheduleJob(existingKey, _tokenSource.Token);
        _triggerKeys.Remove(existingKey);
    }

    public void RemoveAllJobs()
    {
        _scheduler.UnscheduleJobs(_triggerKeys, _tokenSource.Token);
        _triggerKeys.Clear();
    }

    private static string GetTriggerIdentity() => $"sendMessageJobGroupTrigger_{GetGuid()}";

    private static string GetGuid() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());

    private static string GetJobId() => $"botticelliJob_{GetGuid()}";
}