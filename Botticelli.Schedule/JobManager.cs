using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Hangfire;
using Polly;

namespace Botticelli.Scheduler
{
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
                                         () => bot.SendMessageAsync(request,
                                                                    CancellationToken.None),
                                         schedule.Cron);
                return jobId;
            }

            var sendOkPolicy = Policy<SendMessageResponse>
                            .HandleResult(resp => resp.MessageSentStatus != MessageSentStatus.Ok)
                            .WaitAndRetryAsync(reliability.MaxTries,
                                               n => reliability.IsExponential ?
                                                       TimeSpan.FromSeconds(n * Math.Exp(reliability.Delay.TotalSeconds)) :
                                                       reliability.Delay);
            var sendExceptionPolicy = Policy<SendMessageResponse>
                                   .Handle<Exception>()
                                   .WaitAndRetryAsync(reliability.MaxTries,
                                                      n => reliability.IsExponential ?
                                                              TimeSpan.FromSeconds(n * Math.Exp(reliability.Delay.TotalSeconds)) :
                                                              reliability.Delay);

            var sendPolicy = sendExceptionPolicy.WrapAsync(sendOkPolicy);

            RecurringJob.AddOrUpdate(jobId,
                                     () => sendPolicy.ExecuteAsync(() => bot.SendMessageAsync(request,
                                                                CancellationToken.None)),
                                     schedule.Cron);

            return jobId;
        }

        public static void RemoveJob(string jobId) => RecurringJob.RemoveIfExists(jobId);
    }
}