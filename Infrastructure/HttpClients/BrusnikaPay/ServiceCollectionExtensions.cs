using System.Net.Http.Headers;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Payment.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Infrastructure.HttpClients.BrusnikaPay;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrusnikaPay(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient("default").AddResilienceHandler("retry", x =>
        {
            x.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromSeconds(3),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
            });
        });

        services.AddTransient<BrusnikaPayErrorDelegatingHandler>();
        
        services.AddHttpClient<IBrusnikaPayHttpClient, BrusnikaPayHttpClient>(x =>
        {
            var uri = config.GetValue<string>("BrusnikaPay:Uri")
                      ?? throw new ArgumentNullException("BrusnikaPay:Uri");
    
            var token = config.GetValue<string>("BrusnikaPay:Token")
                        ?? throw new ArgumentNullException("BrusnikaPay:Token");

            x.BaseAddress = new Uri(uri);
            x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }).AddHttpMessageHandler<BrusnikaPayErrorDelegatingHandler>();
        
        services.AddSingleton<MerchantData>(x => new MerchantData
        {
            Webhook = config.GetValue<string>("Webhook:Url")
                      ?? throw new ArgumentNullException("Webhook:Url"),
        });

        return services;
    }
}