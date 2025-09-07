using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Application.Caching;

public static class HashExtensions
{
    public static string ToHash(this string input)
    {
        var inputSpan = MemoryMarshal.AsBytes(input.AsSpan());

        Span<byte> resultSpan = stackalloc byte[32];

        if (!SHA256.TryHashData(inputSpan, resultSpan, out var count))
        {
            throw new InvalidOperationException($"Cannot hash the input string: {input}");
        }

        return Convert.ToHexString(resultSpan.Slice(0, count));
    }
}
