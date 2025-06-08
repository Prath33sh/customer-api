using CustomerApi.Models;

namespace CustomerApi.Services;
public interface IUserService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <returns>The created user response.</returns>
    Task<UserResponse> CreateUserAsync(UserRequest request);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user update request.</param>
    /// <returns>The updated user response.</returns>
    Task<UserResponse> UpdateUserAsync(UserUpdateRequest request);

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    Task DeleteUserAsync(Guid userId);

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    Task<UserResponse> GetUserByIdAsync(Guid userId);

}