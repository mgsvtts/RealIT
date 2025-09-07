using Application.Users;
using Application.Users.Dto;
using Application.Users.Dto.GetOperations;
using Application.Users.UserService;
using Domain.Operations;
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
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken token)
    {
        var (user, created) = await _service.GetOrCreateUserAsync(new Login(request.Login), token);

        if (created)
        {
            return Created("", LoginResponse.FromUser(user));
        }
        
        return Ok(LoginResponse.FromUser(user));
    }
    
    [HttpGet("operations")]
    public async Task<ActionResult<CursorPagination<Operation, Guid>>> GetOperations([FromQuery]GetOperationsRequest request, CancellationToken token)
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

        return Ok(operations);
    }
}