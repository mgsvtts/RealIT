using Domain.Users;

namespace Presentation.Controllers.Users.Dto;

public readonly record struct LoginResponse
{
    public Guid Id { get; init; }
    public string Login { get; init; }
    public string AccessToken { get; init; }

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