using Auth1796.Infrastructure.Common.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Auth1796.Infrastructure.Common.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        services.AddApiVersioning();
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings == null) return services;
        if (settings.Enable)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = settings.Title,
                    Version = settings.Version,
                    Contact = new OpenApiContact
                    {
                        Email = settings.ContactEmail,
                        Name = settings.ContactName,
                        Url = new Uri(settings.ContactUrl),
                    },
                    Description = settings.Description,
                    License = new OpenApiLicense
                    {
                        Name = settings.LicenseName,
                        Url = new Uri(settings.LicenseUrl)
                    }
                });

                option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Input your Bearer token to access this API",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                option.SchemaFilter<EnumDescriptionFilter>();
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                option.EnableAnnotations();
            });
        }
        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("SwaggerSettings:Enable"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.DefaultModelsExpandDepth(1);
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                options.EnableTryItOutByDefault();
            });
        }
        return app;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
      services.AddApiVersioning(config =>
      {
          config.DefaultApiVersion = new ApiVersion(1, 0);
          config.AssumeDefaultVersionWhenUnspecified = true;
          config.ReportApiVersions = true;
      });
}