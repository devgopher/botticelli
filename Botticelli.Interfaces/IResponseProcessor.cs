using Botticelli.Shared.API;

namespace Botticelli.Interfaces;

public interface IResponseProcessor
{
    public Task ProcessAsync(BaseResponse response);
}