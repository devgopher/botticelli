using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics;

namespace MetricsRandomGenerator
{
    public class MetricsSender : IHostedService
    {
        private readonly IServiceProvider _sp;
        private readonly MetricsPublisher _publisher;
        private readonly int _threadsCount = 10;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly Random _rand = new Random(DateTime.Now.Microsecond);


        public MetricsSender(IServiceProvider sp)
        {
            _sp = sp;
            _publisher = _sp.GetRequiredService<MetricsPublisher>();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < _threadsCount; i++)
            {
                var thread = new Thread(ThreadProc);
                thread.Start();
            }
        }

        private void ThreadProc()
        {
            var token = _tokenSource.Token;
            if (!token.CanBeCanceled)
                return;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (token.IsCancellationRequested)
                        break;

                    var metric = new MetricObject()
                    {
                        BotId = "TestBot",
                        Name = MetricNames.Names[_rand.Next(0, MetricNames.Names.Length)],
                        Timestamp = DateTime.Now
                    };

                    Console.WriteLine($"Publishing: {metric.BotId}, {metric.Name}, {metric.Timestamp}");

                    var task = _publisher.Publish(metric, token);
                    task.Wait();

                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        } 


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
        }
    }
}
