// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.SemanticKernel.Security;

/// <summary>
/// Provides connector interface for implementing security controls.
/// </summary>
public interface ISecurityConnector
{
    SecurityContext SecurityContext { get; set; }
    /// <summary>
    /// Use this callback for auditing and parameter validation before making an API call.
    /// </summary>
    public void PreRestApiServiceCallback();
    /// <summary>
    /// Use this callback for response validation after making an API call.
    /// </summary>
    public void PostRestApiServiceCallback();
    /// <summary>
    /// Method for clean up and flushing the buffers. Must call this before exiting the application.
    /// </summary>
    public void Close();
}
