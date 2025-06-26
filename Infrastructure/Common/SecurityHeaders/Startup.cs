using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Auth1796.Infrastructure.Common.SecurityHeaders;

internal static class Startup
{
    internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SecurityHeaderSettings)).Get<SecurityHeaderSettings>();

        if (settings?.Enable is true)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Response.HasStarted)
                {

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XXSSProtection))
                    {
                        context.Response.Headers.Add(HeaderNames.XXSSPROTECTION, settings.Headers.XXSSProtection);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.StrictTransportSecurity))
                    {
                        context.Response.Headers.Add(HeaderNames.STRICTTRANSPORTSECURITY, settings.Headers.StrictTransportSecurity);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XFrameOptions))
                    {
                        context.Response.Headers.Add(HeaderNames.XFRAMEOPTIONS, settings.Headers.XFrameOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XContentTypeOptions))
                    {
                        context.Response.Headers.Add(HeaderNames.XCONTENTTYPEOPTIONS, settings.Headers.XContentTypeOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.ContentSecurityPolicy))
                    {
                        context.Response.Headers.Add(HeaderNames.CONTENTSECURITYPOLICY, settings.Headers.ContentSecurityPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.ReferrerPolicy))
                    {
                        context.Response.Headers.Add(HeaderNames.REFERRERPOLICY, settings.Headers.ReferrerPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.FeaturePolicy))
                    {
                        context.Response.Headers.Add(HeaderNames.FEATUREPOLICY, settings.Headers.FeaturePolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.PermissionsPolicy))
                    {
                        context.Response.Headers.Add(HeaderNames.PERMISSIONSPOLICY, settings.Headers.PermissionsPolicy);
                    }

                    /*if (!string.IsNullOrWhiteSpace(settings.Headers.Server))
                    {
                        context.Response.Headers.Add(HeaderNames.SERVER, settings.Headers.Server);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XPoweredBy))
                    {
                        context.Response.Headers.Add(HeaderNames.XPOWEREDBY, settings.Headers.XPoweredBy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.SameSite))
                    {
                        context.Response.Headers.Add(HeaderNames.SAMESITE, settings.Headers.SameSite);
                    }*/
                }

                await next();
            });
        }

        return app;
    }
}