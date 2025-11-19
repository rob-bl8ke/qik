namespace QikApi.Models;

/// <summary>
/// Request model for interpreting a Qik script
/// </summary>
public class InterpretRequest
{
    /// <summary>
    /// The Qik script to interpret
    /// </summary>
    public string Script { get; set; } = string.Empty;

    /// <summary>
    /// Optional variable values to set before interpretation
    /// </summary>
    public Dictionary<string, string>? Variables { get; set; }

    /// <summary>
    /// Optional content encoding (e.g., "base64"). If not specified, assumes plain text.
    /// </summary>
    public string? ContentEncoding { get; set; }
}
