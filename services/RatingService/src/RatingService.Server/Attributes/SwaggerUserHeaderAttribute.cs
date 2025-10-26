using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RatingService.Server.Attributes;

public class SwaggerUserHeaderAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-User-Name",
            In = ParameterLocation.Header,
            Required = true,
            Description = "Имя пользователя",
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}