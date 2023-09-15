using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Audit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Security;

/// <summary>
/// Implements the general purpose functionality for security controls
/// TODO: Use generalized context instead of SecurityContext
/// </summary>
public class SecurityConnector : ISecurityConnector
{
    private static ILogger<IKernel> auditLogger = AuditLoggerFactory.GetLogger();
    public SecurityContext securityContext { get; set; }
    private BlackListService blackListService;

    public SecurityConnector(SecurityContext securityContext)
    {
        this.securityContext = securityContext;
        this.blackListService = new();
    }

    public void PreRestApiServiceCallback()
    {
        // TODO: Add connector logic
        Console.WriteLine("Checking if the endpoint is black-listed");

        if(blackListService.isBlackListed(securityContext.serverUrl))
        {
            securityContext.isBlocked = true;
            AuditLoggerFactory.telemetryClient.TrackEvent("Blocked the REST API call", new Dictionary<string, string>
        {
            { "operation", securityContext.operation },
            { "arguments", string.Join(",", securityContext.arguments) },
            { "serverUrl", securityContext.serverUrl},
            { "apiPath", securityContext.path}
        });
            Console.WriteLine("Endpoint is black-listed. Blocked the endpoint!");

            AuditLoggerFactory.flushChannel();
            return;
        }

        Console.WriteLine("Allowing the endpoint!");

        AuditLoggerFactory.telemetryClient.TrackEvent("Pre REST API call", new Dictionary<string, string>
        {
            { "operation", securityContext.operation },
            { "arguments", string.Join(",", securityContext.arguments) },
            { "serverUrl", securityContext.serverUrl},
            { "apiPath", securityContext.path}
        });
        AuditLoggerFactory.flushChannel();
    }

    public void PostRestApiServiceCallback()
    {
        AuditLoggerFactory.telemetryClient.TrackEvent("Post REST API call", new Dictionary<string, string>
        {
            { "result", securityContext.result.ToString() }
        });
        AuditLoggerFactory.flushChannel();
    }

    private bool isBlackListed(string url)
    {
        return blackListService.isBlackListed(url);
    }
}

