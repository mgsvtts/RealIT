﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Swagger;

public class HealthChecksFilter : IDocumentFilter
{
    private const string HealthCheckEndpoint = "/health";

    public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
    {
        var pathItem = new OpenApiPathItem();
        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = "ApiHealth" });

        var properties = new Dictionary<string, OpenApiSchema>
        {
            { "status", new OpenApiSchema { Type = "string" } },
            { "errors", new OpenApiSchema { Type = "array" } }
        };

        var response = new OpenApiResponse();
        response.Content.Add("application/json", new OpenApiMediaType

        {
            Schema = new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = true,
                Properties = properties,
            }
        });

        operation.Responses.Add("200", response);

        pathItem.AddOperation(OperationType.Get, operation);

        openApiDocument?.Paths.Add(HealthCheckEndpoint, pathItem);
    }
}