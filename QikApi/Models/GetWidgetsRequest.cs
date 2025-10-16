namespace QikApi.Models;

/// <summary>
/// Request model for extracting UI widgets from a Qik script
/// </summary>
public class GetWidgetsRequest
{
    /// <summary>
    /// The Qik script to extract UI widgets from
    /// </summary>
    public string Script { get; set; } = string.Empty;
}
