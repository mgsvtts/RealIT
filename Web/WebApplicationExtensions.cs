using System.Net.Http.Headers;
using Application.Payment;
using Infrastructure.Database;
using Infrastructure.HttpClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Web;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        return builder;
    }

    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("default").AddResilienceHandler("retry", x =>
        {
            x.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromSeconds(3),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
            });
        });

        builder.Services.AddHttpClient<IBrusnikaPayHttpClient, BrusnikaPayHttpClient>(x =>
        {
            var uri = builder.Configuration.GetValue<string>("BrusnikaPay:Uri")
                      ?? throw new ArgumentNullException("BrusnikaPay:Uri");
    
            var token = builder.Configuration.GetValue<string>("BrusnikaPay:Token")
                        ?? throw new ArgumentNullException("BrusnikaPay:Token");

            x.BaseAddress = new Uri(uri);
            x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        });

        builder.Services.AddDbContext<PaymentDbContext>(x=>
        {
            var connection = builder.Configuration.GetValue<string>("ConnectionStrings:Database")
                             ?? throw new ArgumentNullException("ConnectionStrings:Database");

            x.UseNpgsql(connection).UseSnakeCaseNamingConvention();
        });

        builder.Services.AddFusionCache();
        builder.Services.AddStackExchangeRedisCache(x =>
        {
            x.Configuration = builder.Configuration.GetValue<string>("ConnectionStrings:Cache");
        });

        return builder;
    }

    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        return app;
    }
}