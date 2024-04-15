using System.Threading;
using System.Threading.Tasks;

namespace Botticelli.Schedule.Hangfire;

public interface IJob
{
    public string JobName { get; }
    public string JobDescription { get; }
    public Task RunAsync(CancellationToken token);
}