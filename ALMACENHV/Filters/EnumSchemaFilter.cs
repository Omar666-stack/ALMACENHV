using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.Serialization;

namespace ALMACENHV.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                schema.Type = "string";
                schema.Description += "\n\nValores posibles:\n";

                foreach (var enumValue in Enum.GetValues(context.Type))
                {
                    var name = Enum.GetName(context.Type, enumValue);
                    var enumMember = context.Type.GetMember(name!)[0];
                    var enumMemberAttribute = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();
                    
                    var description = enumMemberAttribute != null 
                        ? ((EnumMemberAttribute)enumMemberAttribute).Value 
                        : name;

                    schema.Enum.Add(new OpenApiString(name));
                    schema.Description += $"\n- {name}: {description}";
                }
            }
        }
    }
}
