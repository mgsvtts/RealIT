using Application.Users.Dto.GetOperations;
using Application.Users.TokenService;
using Domain.Operations;
using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Database;
using Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Users.UserService;

public sealed class UsersService(
    PaymentDbContext _db,
    ITokenGenerator _tokenGenerator,
    ILogger<UsersService> _logger) : IUsersService
{
    public async Task<(User User, bool IsCreated)> GetOrCreateUserAsync(Login login, CancellationToken token)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == login, token);

        if (user is not null)
        {
            return (user, false);
        }

        user = User.Create(login);

        var accessToken = _tokenGenerator.Generate(user.Id);

        user.UpdateAccessToken(accessToken);

        _db.Users.Add(user);
            
        await _db.SaveChangesAsync(token);
            
        _logger.LogInformation("User: {Login} not found, created a new one", login);

        return (user, true);
    }

    public async Task<CursorPagination<Operation, Guid>> GetOperationsAsync(GetOperationsDto request, CancellationToken token)
    {
        const int page = 5; //fixed page is easier to cache, should be greater in real world

        var operations = await _db.Operations
            .Where(x => x.UserId == request.UserId)
            .WhereIf(request.PaymentMethod != null, x => x.PaymentMethod == request.PaymentMethod)
            .WhereIf(request.Status != null, x => x.Status == request.Status)
            .WhereIf(request.DateFrom != null, x => x.CreatedAt >= request.DateFrom)
            .WhereIf(request.DateTo != null, x => x.CreatedAt <= request.DateTo)
            .WhereIf(request.Cursor != null && request.CursorDate != null, 
                x => x.CreatedAt < request.CursorDate || (x.CreatedAt == request.CursorDate && x.Id <= request.Cursor))
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(page + 1)
            .ToListAsync(token);
        
        return new CursorPagination<Operation, Guid>(operations, 5);
    }
}