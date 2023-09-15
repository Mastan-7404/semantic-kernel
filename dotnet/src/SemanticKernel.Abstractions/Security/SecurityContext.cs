// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;

namespace Microsoft.SemanticKernel.Security;
public class SecurityContext
{
    public string serverUrl;
    public string path;
    public Dictionary<string, string> arguments;
    public string operation;
    public JsonNode result;
    public bool isBlocked = false;

    public SecurityContext(string operation, string serverUrl, string path, Dictionary<string, string> arguments)
    {
        this.operation = operation;
        this.serverUrl = serverUrl;
        this.path = path;
        this.arguments = arguments;
    }
}
