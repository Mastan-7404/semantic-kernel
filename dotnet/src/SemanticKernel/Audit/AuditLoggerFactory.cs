using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.ApplicationInsights;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Audit;


/// <summary>
/// Example of telemetry in Semantic Kernel using Application Insights within console application.
/// </summary>
public class AuditLoggerFactory
{

    private static LogLevel LogLevel = LogLevel.Trace;
    private const string APP_INSIGHTS_CONNECTION_STRING_VARIABLE = "ApplicationInsights__Key";
    public static TelemetryClient telemetryClient;


    /// <summary>
    /// Example of telemetry in Semantic Kernel using Application Insights within console application.
    /// </summary>
    public static ILogger<IKernel> GetLogger()
    {
        IServiceCollection services = new ServiceCollection();

        // Being a regular console app, there is no appsettings.json or configuration providers enabled by default.
        // Hence instrumentation key/ connection string and any changes to default logging level must be specified here.
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("Category", LogLevel.Information);
            loggingBuilder.SetMinimumLevel(LogLevel);
        });

        services.AddApplicationInsightsTelemetryWorkerService((options) => options.ConnectionString = $"InstrumentationKey=${Environment.GetEnvironmentVariable(APP_INSIGHTS_CONNECTION_STRING_VARIABLE)}");

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        // Obtain logger instance from DI.
        ILogger<IKernel> logger = serviceProvider.GetRequiredService<ILogger<IKernel>>();

        // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
        telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

        return logger;
    }

    public static void flushChannel()
    {
        if (telemetryClient != null)
        {
            telemetryClient.Flush();
            Console.WriteLine("Flush channel");
            Task.Delay(5000).Wait();
        }
    }
}
