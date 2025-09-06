using System.ComponentModel.DataAnnotations;

namespace Domain.Users.ValueObjects;

public readonly record struct Login
{
    public string Value { get; }

    public Login(string value)
    {
        Validate(value);
        
        Value = value;
    }

    private void Validate(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ValidationException("Login should not be empty");
        }

        if (value.Length < 5)
        {
            throw new ValidationException("Login should not be at least 5 characters long");
        }
        
        if (value.Length > 100)
        {
            throw new ValidationException("Login should not be more than 100 characters long");
        }

        if (value.All(x => !char.IsAscii(x)))
        {
            throw new ValidationException("Login should contain only latin alphabet characters");
        }
    }
}