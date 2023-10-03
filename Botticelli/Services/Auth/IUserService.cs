﻿using Botticelli.Server.Data.Entities.Auth;

namespace Botticelli.Server.Services.Auth
{
    public interface IUserService
    {
        /// <summary>
        ///     Registers a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddAsync(UserAddRequest request, CancellationToken token);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateAsync(UserUpdateRequest request, CancellationToken token);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteAsync(UserDeleteRequest request, CancellationToken token);

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserGetResponse> GetAsync(UserGetRequest request, CancellationToken token);

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="requestEmail"></param>
        /// <param name="requestToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmCodeAsync(string requestEmail, string requestToken, CancellationToken token);
    }
}