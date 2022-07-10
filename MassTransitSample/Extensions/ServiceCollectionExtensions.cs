using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MassTransitSample.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            const string swaggerDefinition = "Swagger";
            services.AddSwaggerGen(options => { ConfigureSwaggerGeneration(configuration, swaggerDefinition, options); });
            return services;
        }

        private static void ConfigureSwaggerGeneration(IConfiguration configuration, string swaggerDefinition, SwaggerGenOptions options)
        {
            var definitions = configuration.GetSection(swaggerDefinition).GetChildren()
                .Select(x => x).ToDictionary(x => x.Key, x => configuration.GetSection(x.Path).Get<OpenApiInfo>());

            foreach (var definition in definitions)
                options.SwaggerDoc(definition.Key, definition.Value);

            IncludeLoadXmlComments(options);
        }

        private static void IncludeLoadXmlComments(SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        }

        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}