using System.Security.Claims;
using Application.Users.GetOrCreate;
using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Users;

public interface IUsersService
{
    Task<User> GetOrCreateUserAsync(GetOrCreateUserRequest request, CancellationToken token);
}

public sealed class UsersService(
    PaymentDbContext _db,
    ITokenGenerator _tokenGenerator,
    ILogger<UsersService> _logger) : IUsersService
{
    public async Task<User> GetOrCreateUserAsync(GetOrCreateUserRequest request, CancellationToken token)
    {
        var login = new Login(request.Login);
        
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == login, token);

        if (user is not null)
        {
            return user;
        }

        user = User.Create(login);

        var accessToken = _tokenGenerator.Generate(user.Id);

        user.UpdateAccessToken(accessToken);

        _db.Users.Add(user);
            
        await _db.SaveChangesAsync(token);
            
        _logger.LogInformation("User: {Login} not found, created a new one", login);

        return user;
    }
}