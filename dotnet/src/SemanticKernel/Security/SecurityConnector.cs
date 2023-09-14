using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Audit;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Security;

/// <summary>
/// Implements the general purpose functionality for security controls
/// </summary>
public class SecurityConnector : ISecurityConnector
{
    private static ILogger<IKernel> auditLogger = AuditLoggerFactory.GetLogger();
    public SecurityContext securityContext;

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

    public class SecurityContext
    { 
        public string serverUrl;
        public string path;
        public Dictionary<string, string> arguments;
        public string operation;
        public JsonNode result;

        public SecurityContext(string operation, string serverUrl, string path, Dictionary<string, string> arguments)
        {
            this.operation = operation;
            this.serverUrl = serverUrl;
            this.path = path;
            this.arguments = arguments;
        }
    }
}

