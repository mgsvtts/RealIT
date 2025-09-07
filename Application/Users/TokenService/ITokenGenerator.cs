using Domain.Users.ValueObjects;

namespace Application.Users.TokenService;

/// <summary>
/// Generator for access tokens
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// Generates 10 years long access token
    /// </summary>
    /// <param name="id">UserId to create token to</param>
    /// <returns>access token for the user</returns>
    AccessToken Generate(Guid id);
}