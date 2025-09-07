using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Application.Payment;
using Application.Users;
using Application.Users.TokenService;
using Application.Users.UserService;
using HealthChecks.UI.Client;
using Infrastructure.Database;
using Infrastructure.HttpClients;
using Infrastructure.HttpClients.BrusnikaPay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Presentation.BackgroundServices;
using Presentation.Filters;
using Serilog;
using Web.HealthChecks;
using Web.Swagger;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Web;

/// <summary>
/// Utility class to simplify Program.cs
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerDocumentation();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = builder.Configuration.GetValue<string>("Jwt:Key")
                          ?? throw new ArgumentNullException("Jwt:Key");

                var iss = builder.Configuration.GetValue<string>("Jwt:Iss")
                          ?? throw new ArgumentNullException("Jwt:Iss");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = iss,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        builder.Services.AddControllers();

        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddHostedService<SynchronizeOperationJob>();
        builder.Services.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        return builder;
    }

    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPaymentService, PaymentService>();

        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<IUsersService, CachedUserService>();

        builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>(x =>
        {
            var key = builder.Configuration.GetValue<string>("Jwt:Key")
                      ?? throw new ArgumentNullException("Jwt:Key");

            var iss = builder.Configuration.GetValue<string>("Jwt:Iss")
                      ?? throw new ArgumentNullException("Jwt:Iss");

            return new TokenGenerator(Encoding.UTF8.GetBytes(key), iss);
        });

        return builder;
    }

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddBrusnikaPay(builder.Configuration);

        var dbConnection = builder.Configuration.GetConnectionString("Database")
                           ?? throw new ArgumentNullException("ConnectionStrings:Database");

        var cacheConnection = builder.Configuration.GetConnectionString("Cache")
                              ?? throw new ArgumentNullException("ConnectionStrings:Cache");

        builder.Services.AddDbContext<PaymentDbContext>(x =>
        {
            x.UseNpgsql(dbConnection).UseSnakeCaseNamingConvention();
        });

        builder.Services
            .AddFusionCache()
            .WithDistributedCache(_ => new RedisCache(new RedisCacheOptions
            {
                Configuration = cacheConnection
            })).WithSerializer(new FusionCacheSystemTextJsonSerializer());

        builder.Services.AddSerilog(x =>
        {
            x.WriteTo.Console();

            x.ReadFrom.Configuration(builder.Configuration);
        });

        builder.Services.AddHealthChecks()
            .AddCheck<BruskinaPayHealthCheck>("brusnika-pay")
            .AddNpgSql(dbConnection)
            .AddRedis(cacheConnection);

        return builder;
    }

    public static WebApplication RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

        db.Database.Migrate();

        return app;
    }

    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks(
            "/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        app.MapControllers();

        return app;
    }
}