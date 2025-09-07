using Application.Users.Dto.GetOperations;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users;

/// <summary>
/// Service for handling users and their operations
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Gets or creates user with provided Login
    /// </summary>
    /// <param name="login">Login to find</param>
    /// <returns>User and IsCreated marker</returns>
    Task<(User User, bool IsCreated)> GetOrCreateUserAsync(Login login, CancellationToken token);
    
    /// <summary>
    /// Gets operations connected with provided user using cursor pagination
    /// </summary>
    /// <returns>Cursor paginated operations</returns>
    Task<CursorPagination<Guid>> GetOperationsAsync(GetOperationsDto request, CancellationToken token);
}