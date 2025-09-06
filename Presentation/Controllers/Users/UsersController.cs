using Application.Users;
using Application.Users.GetOrCreate;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Users.Dto;

namespace Presentation.Controllers.Users;

[ApiController]
[Route("v1/users")]
public sealed class UsersController(IUsersService _service) : ControllerBase
{
    [HttpPut]
    public async Task<LoginResponse> Login(GetOrCreateUserRequest request, CancellationToken token)
    {
        var user = await _service.GetOrCreateUserAsync(request, token);
        
        return LoginResponse.FromUser(user);
    }
}