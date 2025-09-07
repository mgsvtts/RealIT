using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.BrusnikaPay.Dto.Operations;

public readonly record struct GetOperationsRequest
{
    [JsonPropertyName("take")]
    public int Take { get; init; }
    
    [JsonPropertyName("skip")]
    public int Skip { get; init; }
}