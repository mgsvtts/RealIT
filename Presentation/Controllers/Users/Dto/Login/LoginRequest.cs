using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers.Users.Dto.Login;

public sealed class LoginRequest
{
    [Required] 
    public string Login { get; init; } = "";
}