using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleProject.WebApi.Swagger
{
    public class SwaggerCustomeHeaders : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "CustomeHeader",
            //    In = ParameterLocation.Header,
            //    AllowEmptyValue = true,
            //    Required = false
            //});
        }
    }
}

