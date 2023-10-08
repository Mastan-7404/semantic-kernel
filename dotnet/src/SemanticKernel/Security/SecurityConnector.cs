using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.SemanticKernel.Audit;

namespace Microsoft.SemanticKernel.Security;

/// <summary>
/// Implements the general purpose functionality for security controls
/// TODO: Use generalized context instead of SecurityContext
/// </summary>
public class SecurityConnector : ISecurityConnector
{
    private TelemetryClient auditTelemetryClient;
    public SecurityContext SecurityContext { get; set; }
    private BlackListService blackListService;

    public SecurityConnector(SecurityContext securityContext)
    {
        this.SecurityContext = securityContext;
        this.auditTelemetryClient = AuditTelemetryClientFactory.GetTelemetryClient();
        this.blackListService = new();
    }

    ~SecurityConnector()
    {
        // This is a hack. There needs to be a more elegant way to flush the telemetry client.
        Close();
    }

    public void PreRestApiServiceCallback()
    {
        var preApiCallContextParameters = new Dictionary<string, string>
        {
            { "operation", SecurityContext.operation },
            { "arguments", string.Join(",", SecurityContext.arguments) },
            { "serverUrl", SecurityContext.serverUrl},
            { "apiPath", SecurityContext.path}
        };
        // TODO: Add connector logic
        auditTelemetryClient.TrackEvent("Pre REST API call", preApiCallContextParameters);

        Console.WriteLine($"Checking if the url {SecurityContext.serverUrl} is black-listed");

        if(blackListService.isBlackListed(SecurityContext.serverUrl))
        {
            SecurityContext.isBlocked = true;
            auditTelemetryClient.TrackEvent("Blocked the REST API call", preApiCallContextParameters);
            Console.WriteLine("Endpoint is black-listed. Blocked the endpoint!");

            return;
        }

        Console.WriteLine("Allowing the endpoint!");
    }

    public void PostRestApiServiceCallback()
    {
        auditTelemetryClient.TrackEvent("Post REST API call", new Dictionary<string, string>
        {
            { "result", SecurityContext.result.ToString() }
        });
    }

    private bool IsBlackListed(string url)
    {
        return blackListService.isBlackListed(url);
    }

    public void Close()
    {
        FlushTelemetryClient();
    }

    private void FlushTelemetryClient()
    {
        auditTelemetryClient.Flush();
        Task.Delay(5000).Wait();
    }
}

