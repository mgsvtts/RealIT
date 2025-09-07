using Infrastructure.HttpClients.BrusnikaPay;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Web.HealthChecks;

public class BruskinaPayHealthCheck(IBrusnikaPayHttpClient _client) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            var banks = await _client.GetBankListAsync(cancellationToken);
            
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Error from BrusnikaPay", ex);

        }
    }
}