namespace QikApi.Models;

/// <summary>
/// Request model for extracting UI widgets from a Qik script
/// </summary>
public class GetWidgetsRequest
{
    /// <summary>
    /// The Qik script to extract UI widgets from (Base64 encoded if ContentEncoding is "base64", otherwise plain text)
    /// </summary>
    public string Script { get; set; } = string.Empty;

    /// <summary>
    /// Optional content encoding format. If "base64", the script will be decoded from Base64. Otherwise, plain text is expected.
    /// </summary>
    public string? ContentEncoding { get; set; }
}
