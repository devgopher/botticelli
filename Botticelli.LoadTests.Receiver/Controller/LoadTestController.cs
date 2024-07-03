using System.Diagnostics;
using Botticelli.LoadTests.Receiver.Models;
using Botticelli.LoadTests.Receiver.Result;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.LoadTests.Receiver.Controller;

[ApiController]
[Route("/load-tests")]
public class LoadTestController(ILoadTestGate loadTestGate)
{
    [HttpPost("[action]")]
    public async Task<CommandResult> SendCommandResponse([FromBody]SendCommandRequestModel model)
    {
        var cts = new CancellationTokenSource();
        var stopWatch = Stopwatch.StartNew();
        CommandResult? result;
        
        try
        {
            var task = loadTestGate.ThrowCommand(model.Command, model.Args ?? string.Empty, cts.Token);
            
            result = loadTestGate.WaitForExecution(task, model.Timeout, cts.Token).Result;
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