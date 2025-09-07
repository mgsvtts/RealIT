using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Application.Payment;
using Application.Users;
using Infrastructure.Database;
using Infrastructure.HttpClients;
using Infrastructure.HttpClients.BrusnikaPay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Presentation.BackgroundServices;
using Presentation.Filters;
using Serilog;

namespace Web;

/// <summary>
/// Utility class to simplify Program.cs
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter Bearer token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

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
        builder.Services.AddScoped<IUsersService, UsersService>();
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

        builder.Services.AddDbContext<PaymentDbContext>(x=>
        {
            var connection = builder.Configuration.GetValue<string>("ConnectionStrings:Database")
                             ?? throw new ArgumentNullException("ConnectionStrings:Database");

            x.UseNpgsql(connection).UseSnakeCaseNamingConvention();
        });

        builder.Services.AddSerilog(x =>
        {
            x.WriteTo.Console();
            
            x.ReadFrom.Configuration(builder.Configuration);
        });

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
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        return app;
    }
}