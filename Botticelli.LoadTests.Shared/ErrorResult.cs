namespace Botticelli.LoadTests.Shared;

public class ErrorResult : IStepResult
{
    public bool IsSuccess => false;
    public string Error { get; init; }
}

public class BaseResult : IStepResult
{
    public bool IsSuccess { get; init; }
    public string Error { get; init; }
    public string Message { get; init; }
}