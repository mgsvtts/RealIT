using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers;

public static class HttpContextExtensions
{
    /// <summary>
    /// Gets UserId from Bearer token
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <returns>UserId from Bearer token</returns>
    /// <exception cref="ValidationException">if failed to get UserId</exception>
    public static Guid GetUserIdOrThrow(this HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new ValidationException("Failed to parse userId from Bearer header");
        }

        return Guid.Parse(userId);
    }
}