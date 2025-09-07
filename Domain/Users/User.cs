using Domain.Operations;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User
{
    public Guid Id { get; private set; }
    public Login Login { get; private set; }
    public AccessToken AccessToken { get; private set; }
    public IReadOnlyList<Operation> Operations { get; private set; } = null!;

    public static User Create(Login login)
    {
        return new User
        {
            Id = Guid.CreateVersion7(),
            Login = login,
            AccessToken = default,
            Operations = []
        };
    }

    public void UpdateAccessToken(AccessToken token)
    {
        AccessToken = token;
    }
}