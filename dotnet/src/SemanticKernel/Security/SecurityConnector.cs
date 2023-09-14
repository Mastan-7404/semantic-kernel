using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Audit;
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

    public SecurityConnector(SecurityContext securityContext)
    {
        this.securityContext = securityContext;
    }

    public void PreRestApiServiceCallback()
    {
        // TODO: Add connector logic
        AuditLoggerFactory.telemetryClient.TrackEvent("Pre REST API call", new Dictionary<string, string>
        {
            { "operation", securityContext.operation },
            { "arguments", string.Join(",", securityContext.arguments) },
            { "serverUrl", securityContext.serverUrl},
            { "apiPath", securityContext.path}
        });
        AuditLoggerFactory.telemetryClient.Flush();
        Task.Delay(2000).Wait();
    }

    public void PostRestApiServiceCallback()
    {
        AuditLoggerFactory.telemetryClient.TrackEvent("Post REST API call", new Dictionary<string, string>
        {
            { "result", securityContext.result.ToString() }
        });
        AuditLoggerFactory.telemetryClient.Flush();
        Task.Delay(2000).Wait();
    }
}

