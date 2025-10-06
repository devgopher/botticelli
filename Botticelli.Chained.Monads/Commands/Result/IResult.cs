using System.ComponentModel.DataAnnotations;
using Botticelli.Framework.Commands;

namespace Botticelli.Chained.Monads.Commands.Result;

public interface IResult<out TCommand>
        where TCommand : ICommand
{
    [Required]
    public bool IsSuccess { get; }

    [Required]
    public TCommand Command { get; }
}