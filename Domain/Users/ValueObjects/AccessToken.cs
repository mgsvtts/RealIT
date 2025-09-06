using System.ComponentModel.DataAnnotations;

namespace Domain.Users.ValueObjects;

public readonly record struct AccessToken
{
    public string Value { get; }

    public AccessToken(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ValidationException("AccessToken should not be empty");
        }

        Value = value;
    }
}