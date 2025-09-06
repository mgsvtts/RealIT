using System.ComponentModel.DataAnnotations;

namespace Application.Users.GetOrCreate;

public sealed class GetOrCreateUserRequest
{
    [Required] 
    public string Login { get; init; } = "";
}