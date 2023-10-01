using Botticelli.Server.Data.Entities.Auth;

namespace Botticelli.Server.Services.Auth
{
    public interface IUserService
    {
        /// <summary>
        ///     Registers a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task RegisterAsync(UserAddRequest request);
        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateAsync(UserUpdateRequest request);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DeleteAsync(UserDeleteRequest request);

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserGetResponse> GetAsync(UserGetRequest request);
    }
}
