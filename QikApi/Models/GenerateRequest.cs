using System.ComponentModel.DataAnnotations;

namespace QikApi.Models;

/// <summary>
/// Request model for the generate endpoint
/// </summary>
public class GenerateRequest
{
    /// <summary>
    /// Base64 encoded string of the Qik script
    /// </summary>
    [Required]
    public string Script { get; set; } = string.Empty;

    /// <summary>
    /// Dictionary of fragments where values are Base64 encoded content
    /// </summary>
    [Required]
    public Dictionary<string, string> Fragments { get; set; } = new();

    /// <summary>
    /// Dictionary of documents built from referenced fragment content
    /// </summary>
    [Required]
    public Dictionary<string, string> Documents { get; set; } = new();

    /// <summary>
    /// Dictionary of Qik input variables and their values
    /// </summary>
    public Dictionary<string, string> Inputs { get; set; } = new();

    /// <summary>
    /// Prefix for placeholder variables (default: "@{")
    /// </summary>
    public string PlaceholderPrefix { get; set; } = "@{";

    /// <summary>
    /// Suffix for placeholder variables (default: "}")
    /// </summary>
    public string PlaceholderSuffix { get; set; } = "}";

    /// <summary>
    /// Optional content encoding format. If "base64", the script and fragments will be decoded from Base64. Otherwise, plain text is expected.
    /// </summary>
    public string? ContentEncoding { get; set; }
}