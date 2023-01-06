using Botticelli.Shared.API;

namespace Botticelli.Interfaces;

public interface IClientResponseProcessor : IResponseProcessor
    where T : BaseResponse
{
}