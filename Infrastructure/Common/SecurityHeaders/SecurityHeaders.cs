namespace Auth1796.Infrastructure.Common.SecurityHeaders;
public class SecurityHeaders
{
    /// <summary>
    /// X-XSS-Protection header.
    /// </summary>
    public string XXSSProtection { get; set; }

    /// <summary>
    /// Strict-Transport-Security header.
    /// </summary>
    public string StrictTransportSecurity { get; set; }

    /// <summary>
    /// X-Frame-Options header.
    /// </summary>
    public string XFrameOptions { get; set; }

    /// <summary>
    /// X-Content-Type-Options header.
    /// </summary>
    public string XContentTypeOptions { get; set; }

    /// <summary>
    /// Content-Security-Policy header.
    /// </summary>
    public string ContentSecurityPolicy { get; set; }

    /// <summary>
    /// Referrer-Policy header.
    /// </summary>
    public string ReferrerPolicy { get; set; }

    /// <summary>
    /// Feature-Policy header.
    /// </summary>
    public string FeaturePolicy { get; set; }

    /// <summary>
    /// Permissions-Policy header.
    /// </summary>
    public string PermissionsPolicy { get; set; }

    /// <summary>
    /// Server header.
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// x-powered-by header.
    /// </summary>
    public string XPoweredBy { get; set; }

    /// <summary>
    /// SameSite attribute for cookies.
    /// </summary>
    public string SameSite { get; set; }
}