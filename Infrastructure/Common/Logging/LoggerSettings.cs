namespace Auth1796.Infrastructure.Common.Logging;

public class LoggerSettings
{
    public string AppName { get; set; } = "WebAPI";
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = true;
    public string FilePath { get; set; } = "Logs/logs.json";
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}