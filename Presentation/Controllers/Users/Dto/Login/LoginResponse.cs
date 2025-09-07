using Domain.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Users.Dto.Login;

public readonly record struct LoginResponse
{
    [SwaggerSchema("Id of the user")]
    public required Guid Id { get; init; }
    
    [SwaggerSchema("Login that was provided")]
    public required string Login { get; init; }
    
    [SwaggerSchema("10 years long access token")]
    public required string AccessToken { get; init; }

    public static LoginResponse FromUser(User user)
    {
        return new LoginResponse
        {
            Id = user.Id,
            Login = user.Login.Value,
            AccessToken = user.AccessToken.Value
        };
    }
}