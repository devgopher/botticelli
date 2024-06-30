using System.Diagnostics;
using Botticelli.LoadTests.Receiver.Result;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.LoadTests.Receiver.Controller;

[ApiController]
// [Route("/load-tests")]
public class LoadTestController(ILoadTestGate loadTestGate)
{
    [HttpGet("[action]")]
    public async Task<CommandResult> GetResponse(string command, string? args, TimeSpan timeout)
    {
        var cts = new CancellationTokenSource();
        var stopWatch = Stopwatch.StartNew();
        CommandResult? result;
        try
        {
            var task = loadTestGate.ThrowCommand(command, args ?? string.Empty, cts.Token);
            
            result = loadTestGate.WaitForExecution(task, timeout, cts.Token).Result;
        }
        catch (Exception ex)
        {
            result = new CommandResult
            {
                ResultMessage = $"Error {ex.Message}",
                Duration = stopWatch.Elapsed,
                IsSuccess = false
            };
        }
        finally
        {
            stopWatch.Stop();
            await cts.CancelAsync();
        }

        return result;
    }
}