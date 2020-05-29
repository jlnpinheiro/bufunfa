using JNogueira.Bufunfa.Dominio;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Api.Swagger.Filters
{
    public class EnumDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Operations, swaggerDoc);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc)
        {
            if (operations != null)
            {
                foreach (var oper in operations)
                {
                    foreach (var param in oper.Value.Parameters)
                    {
                        var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == param.Name);
                        if (paramEnum.Value != null)
                        {
                            param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                        }
                    }
                }
            }
        }

        private Type GetEnumTypeByName(string enumTypeName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name == enumTypeName);
        }

        private string DescribeEnum(IList<IOpenApiAny> enums, string proprtyTypeName)
        {
            List<string> enumDescriptions = new List<string>();
            
            var enumType = GetEnumTypeByName(proprtyTypeName);
            
            if (enumType == null)
                return null;

            foreach (OpenApiInteger enumOption in enums)
            {
                if (enumType.Name.EndsWith("OrdenarPor"))
                {
                    enumDescriptions.Add($"\"{(Enum)Enum.ToObject(enumType, enumOption.Value)}\" = {((Enum)Enum.ToObject(enumType, enumOption.Value)).ObterDescricao()}");
                    continue;
                }

                if (Convert.ToInt32(enumOption.Value) != -1)
                    enumDescriptions.Add($"{enumOption.Value} = {((Enum)Enum.ToObject(enumType, enumOption.Value)).ObterDescricao()}");
            }

            return $"<ul><li>{string.Join("</li><li>", enumDescriptions)}</li></ul>";
        }
    }
}
