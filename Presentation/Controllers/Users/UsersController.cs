using Application.Users;
using Application.Users.Dto;
using Application.Users.Dto.GetOperations;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Users.Dto;
using Presentation.Controllers.Users.Dto.Login;
using Presentation.Controllers.Users.Dto.Operations;

namespace Presentation.Controllers.Users;

[Authorize]
[ApiController]
[Route("v1/users")]
public sealed class UsersController(IUsersService _service) : ControllerBase
{
    [HttpPut]
    [AllowAnonymous]
    public async Task<IResult> Login(LoginRequest request, CancellationToken token)
    {
        var (user, created) = await _service.GetOrCreateUserAsync(new Login(request.Login), token);

        if (created)
        {
            return Results.Created("", LoginResponse.FromUser(user));
        }
        
        return Results.Ok(LoginResponse.FromUser(user));
    }
    
    [HttpGet("operations")]
    public async Task<IResult> GetOperations([FromQuery]GetOperationsRequest request, CancellationToken token)
    {
        var operations = await _service.GetOperationsAsync(new GetOperationsDto
        {
            Cursor = request.Cursor,
            CursorDate = request.CursorDate,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            PaymentMethod = request.PaymentMethod,
            Status = request.Status,
            UserId = HttpContext.GetUserIdOrThrow()
        }, token);

        return Results.Ok(operations);
    }
}