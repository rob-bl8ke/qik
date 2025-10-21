namespace QikApi.Models;

/// <summary>
/// Response model for the generate endpoint
/// </summary>
public class GenerateResponse
{
    /// <summary>
    /// Indicates whether the generation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Dictionary of generated documents where key is the document name and value is the Base64 encoded content
    /// </summary>
    public Dictionary<string, string> Documents { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Error message if generation failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Additional metadata about the generation process
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
}