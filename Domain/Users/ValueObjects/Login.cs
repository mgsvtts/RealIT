using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Users.ValueObjects;

public static partial class Regexes
{
    [GeneratedRegex(@"^[\x20-\x7E]+$", RegexOptions.Compiled, 5_000)]
    public static partial Regex IsAscii();
}

public readonly record struct Login
{
    public string Value { get; }

    public Login(string value)
    {
        value = value.Trim();
        
        Validate(value);
        
        Value = value.ToLower();
    }

    private static void Validate(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ValidationException("Login should not be empty");
        }

        if (value == "string")
        {
            throw new ValidationException("Choose something more creative as your login please");
        }
        
        if (value.Length < 5)
        {
            throw new ValidationException("Login should be at least 5 characters long");
        }
        
        if (value.Length > 100)
        {
            throw new ValidationException("Login should not be more than 100 characters long");
        }

        if (!Regexes.IsAscii().IsMatch(value))
        {
            throw new ValidationException("Login should contain only latin alphabet characters");
        }
    }
}