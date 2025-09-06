using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Infrastructure.HttpClients.Dto;
using Infrastructure.HttpClients.Dto.Payment;
using Infrastructure.HttpClients.Dto.Payment.Request;

namespace Infrastructure.HttpClients;

public interface IBrusnikaPayHttpClient
{
    Task<BrusnikaPayResponse<PaymentResponse>> FullPaymentAsync(FullPaymentRequest request, CancellationToken token);
}

public sealed class BrusnikaPayHttpClient(HttpClient _httpClient) : IBrusnikaPayHttpClient
{
    public async Task<BrusnikaPayResponse<PaymentResponse>> FullPaymentAsync(FullPaymentRequest request, CancellationToken token)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json),
            RequestUri = new Uri(_httpClient.BaseAddress + "paymentform/full"),
        };

        var response = await _httpClient.SendAsync(message, token);

        return await response.Content.ReadFromJsonAsync<BrusnikaPayResponse<PaymentResponse>>(token);
    }
    
    public async Task<BrusnikaPayResponse<PaymentResponse>> PreparePaymentAsync(PreparePaymentRequest request, CancellationToken token)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json),
            RequestUri = new Uri(_httpClient.BaseAddress + "paymentform/prepare"),
        };

        var response = await _httpClient.SendAsync(message, token);

        return await response.Content.ReadFromJsonAsync<BrusnikaPayResponse<PaymentResponse>>(token);
    }
}