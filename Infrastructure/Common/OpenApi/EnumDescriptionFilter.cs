using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Auth1796.Infrastructure.Common.OpenApi;
public class EnumDescriptionFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumType = context.Type;
            string[] names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            var description = new StringBuilder();
            description.AppendLine("Enum values:");
            for (int i = 0; i < names.Length; i++)
            {
                description.AppendLine($"- {names[i]} ({values.GetValue(i)})");
            }
            schema.Description = description.ToString();
        }
    }
}