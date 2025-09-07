using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Users.Dto.Login;

public readonly record struct LoginRequest
{
    [Required] 
    [SwaggerSchema("Unique login of the user (will be converted to lower case)")]
    public string Login { get; init; }
}