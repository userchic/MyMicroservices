using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection.Metadata;

namespace AuthService.SwaggerFilters
{
    public class AuthHeaderFilter:IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null) return;

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IOpenApiParameter>();
            }

            var parameter = new OpenApiParameter
            {
                Description = "JWT токен",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Required = false,
                Schema = new OpenApiSchema() { Type = JsonSchemaType.String }
            };
            operation.Parameters.Add(parameter);
            parameter = new OpenApiParameter
            {
                Description = "JWT токен2",
                In = ParameterLocation.Header,
                Name = "MyAuth",
                Required = false,
                Schema = new OpenApiSchema() { Type = JsonSchemaType.String }
            };
            operation.Parameters.Add(parameter);

        }
    }
}
