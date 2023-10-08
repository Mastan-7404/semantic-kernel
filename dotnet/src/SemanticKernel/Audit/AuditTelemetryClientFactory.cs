using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights;

namespace Microsoft.SemanticKernel.Audit;


/// <summary>
/// Example of telemetry in Semantic Kernel using Application Insights within console application.
/// </summary>
public class AuditTelemetryClientFactory
{
    private const string APP_INSIGHTS_KEY_VARIABLE = "ApplicationInsights__Key";

    /// <summary>
    /// Example of telemetry in Semantic Kernel using Application Insights within console application.
    /// </summary>
    public static TelemetryClient GetTelemetryClient()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddApplicationInsightsTelemetryWorkerService((options) => options.ConnectionString = $"InstrumentationKey={Environment.GetEnvironmentVariable(APP_INSIGHTS_KEY_VARIABLE)}");

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
        TelemetryClient telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

        return telemetryClient;
    }
}
