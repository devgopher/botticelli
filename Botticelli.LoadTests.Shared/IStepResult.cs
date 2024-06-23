namespace Botticelli.LoadTests.Shared;

public interface IStepResult : IStepInput
{
    public bool IsSuccess { get; }
    public string Error { get; init; }
}