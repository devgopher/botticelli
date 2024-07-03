namespace Botticelli.LoadTests.Receiver.Models;

public record SendCommandRequestModel(string Command, string? Args, TimeSpan Timeout);