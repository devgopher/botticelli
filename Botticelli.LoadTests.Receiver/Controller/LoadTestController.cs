using System.Diagnostics;
using System.Web.Mvc;
using Botticelli.Framework.Commands;
using Botticelli.LoadTests.Receiver.Result;
using Botticelli.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.LoadTests.Receiver.Controller;

[Authorize(AuthenticationSchemes = "Bearer")]
public class LoadTestController(ILoadTestGate loadTestGate) : ControllerBase
{
    public async Task<CommandResult> GetResponse(ICommand command, TimeSpan timeout)
    {
        var stopWatch = Stopwatch.StartNew();
        var result = new CommandResult();
        try
        {
            var messageUid = await loadTestGate.ThrowCommand(command);
            result.ResultMessage = await loadTestGate.WaitForResponse(messageUid, timeout);
            result.Duration = stopWatch.Elapsed;
            result.IsSuccess = false;
        }
        catch (Exception)
        {
            result.ResultMessage = new Message();
            result.Duration = stopWatch.Elapsed;
            result.IsSuccess = false;
        }
        finally
        {
            stopWatch.Stop();
        }

        return result;
    }

    protected override void ExecuteCore()
    {
        throw new NotImplementedException();
    }
}