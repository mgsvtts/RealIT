using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.BrusnikaPay.Dto;

public enum PayStatus
{
    Success,
    Warning,
    InternalError,
    AuthError,
    AccessError
}

public readonly record struct BrusnikaPayResponse<T> where T : notnull
{
    [JsonPropertyName("result")]
    public BrusnikaPayResult Result { get; init; }
    
    [JsonPropertyName("data")]
    public T Data { get; init; }
    
    [JsonPropertyName("totalNumberRecords")]
    public int TotalNumberRecords { get; init; }

    public bool IsSuccess() => Result.Status == PayStatus.Success;
}

public readonly record struct BrusnikaPayResult
{
    [JsonPropertyName("status")] 
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public PayStatus Status { get; init; }
    
    [JsonPropertyName("x-request-id")]
    public string RequestId { get; init; }
    
    [JsonPropertyName("code-error")]
    public string CodeError { get; init; }
    
    [JsonPropertyName("message")]
    public string Message { get; init; } 
}