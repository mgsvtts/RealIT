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
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Users;

[Authorize]
[ApiController]
[Route("v1/users")]
public sealed class UsersController(IUsersService _service) : ControllerBase
{
    [HttpPut]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Gets or creates a new user with provided login")]
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
    [SwaggerOperation(Summary = "Gets user operations using cursor pagination")]
    public async Task<ActionResult<CursorPagination<OperationResponseItem, Guid>>> GetOperations([FromQuery]GetOperationsRequest request, CancellationToken token)
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

        return Ok(operations.MapUsing(OperationResponseItem.FromOperation));
    }
}