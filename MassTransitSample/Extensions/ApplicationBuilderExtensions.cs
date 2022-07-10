using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MassTransitSample.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            const string swaggerJsonPath = "/swagger/{0}/swagger.json";
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                GetApiVersions(configuration).ForEach(apiVersion =>
                    c.SwaggerEndpoint(string.Format(swaggerJsonPath, apiVersion), apiVersion));
            });
            return app;
        }

        private static List<string> GetApiVersions(IConfiguration configuration)
        {
            const string swaggerDefinition = "Swagger";
            return configuration.GetSection(swaggerDefinition).GetChildren().Select(x => x.Key).ToList();
        }
    }
}