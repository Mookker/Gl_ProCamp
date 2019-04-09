using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommonLibrary.Helpers
{
    public class AppSetup
    {
        
        /// <summary>
        /// Setups swagger
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiName"></param>
        /// <param name="hostingEnv"></param>
        public static void SetupSwagger(SwaggerGenOptions options, string apiName, IHostingEnvironment hostingEnv)
        {
            options.SwaggerDoc("v1", new Info
            {
                Title = apiName,
                Description = $"{apiName} (ASP.NET Core 2.2)",
                Version = "1.0.BUILD_NUMBER"
            });

            options.DescribeAllEnumsAsStrings();

            var comments =
                new XPathDocument(
                    $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
            options.OperationFilter<XmlCommentsOperationFilter>(comments);

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey",
            });
            options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            {
                {"Bearer", Enumerable.Empty<string>()}
            });
            options.IgnoreObsoleteActions();
        }
    }
}