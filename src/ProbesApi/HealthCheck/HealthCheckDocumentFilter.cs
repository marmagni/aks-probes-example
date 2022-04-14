using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProbesApi.HealthCheck
{
    public class HealthCheckDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Add("/alive", new OpenApiPathItem()
            {
                Operations = GenerateOperations("alive")
            });

            swaggerDoc.Paths.Add("/warmup", new OpenApiPathItem()
            {
                Operations = GenerateOperations("prepared")
            });

            swaggerDoc.Paths.Add("/health", new OpenApiPathItem()
            {
                Operations = GenerateOperations("healthy to receive request")
            });
        }

        private static Dictionary<OperationType, OpenApiOperation> GenerateOperations(string description)
        {
            var operations = new Dictionary<OperationType, OpenApiOperation>();

            var responses = new OpenApiResponses
            {
                {
                    "200",
                    new OpenApiResponse()
                    {
                        Description = $"API is {description}"
                    }
                },
                {
                    "503",
                    new OpenApiResponse()
                    {
                        Description = $"API is not {description}"
                    }
                }
            };

            operations.Add(OperationType.Get, new OpenApiOperation()
            {
                Responses = responses,
                Tags = new List<OpenApiTag>
                {
                    new() { Name = "Probes" }
                }                
            });

            return operations;
        }
    }
}
