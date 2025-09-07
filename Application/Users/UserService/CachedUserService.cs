using System.Text.Json;
using Application.Hashing;
using Application.Users.Dto.GetOperations;
using Domain.Operations;
using Domain.Users;
using Domain.Users.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Application.Users.UserService;

public sealed class CachedUserService(UsersService _decoratee, IFusionCache _cache) : IUsersService
{
    private static readonly FusionCacheEntryOptions _options = new()
    {
        Duration = TimeSpan.FromSeconds(30)
    };
    
    public Task<(User User, bool IsCreated)> GetOrCreateUserAsync(Login login, CancellationToken token)
    {
        return _decoratee.GetOrCreateUserAsync(login, token);
    }

    public async Task<CursorPagination<Operation, Guid>> GetOperationsAsync(GetOperationsDto request, CancellationToken token)
    {
        var key = "operations-" + JsonSerializer.Serialize(request).ToHash();

        var operations = await _cache.GetOrSetAsync(
            key,
            async _ => await _decoratee.GetOperationsAsync(request, token),
            options: _options,
            token: token
        );

        return operations;
    }
}