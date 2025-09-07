using System.Text.Json;
using Infrastructure.HttpClients.BrusnikaPay.Dto;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HttpClients.BrusnikaPay;

public class BrusnikaPayErrorDelegatingHandler(ILogger<BrusnikaPayErrorDelegatingHandler> _logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unsuccessful code from BruskinaPay: {@Request}", request);

            throw new HttpRequestException($"Got error response from BrusnikaPay");
        }
        
        await response.Content.LoadIntoBufferAsync(cancellationToken);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        var body = JsonSerializer.Deserialize<BrusnikaPayResponse<object>>(stream);

        stream.Seek(0, SeekOrigin.Begin); 

        if (body.IsSuccess())
        {
            return response;
        }

        _logger.LogError(
            "Got error from BrusnikaPay: {PaymentStatus}:{ErrorCode}:{Message}",
            body.Result.Status,
            body.Result.CodeError,
            body.Result.Message
        );

        throw new HttpRequestException($"Got error response from BrusnikaPay, state: {body.Result.Status}, message: {body.Result.Message}");
    }
}