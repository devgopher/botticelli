namespace Botticelli.LoadTests.Shared;

public class ErrorResult : IStepResult
{
    public bool IsSuccess => false;
    public string Error { get; init; }
}