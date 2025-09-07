using Microsoft.OpenApi.Models;

namespace Web.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.EnableAnnotations();
            
            opt.DocumentFilter<HealthChecksFilter>();

            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RealIT API",
                Version = "v1",
                Description = 
                """
                ## Flow description
                #### 1. Authorize using ``PUT /v1/users``
                #### 2. Create payment using ``POST v1/payments``
                #### 3. Find a new operation in ``GET v1/users/operations``
                #### 4. OPTIONAL: change status of this operation in ``POST /webhook``
                """
            });
            
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

        return services;
    }
}