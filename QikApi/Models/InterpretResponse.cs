namespace QikApi.Models;

/// <summary>
/// Response model containing interpretation results
/// </summary>
public class InterpretResponse
{
    /// <summary>
    /// Dictionary of all variable names and their resolved values
    /// </summary>
    public Dictionary<string, string> Values { get; set; } = new();

    /// <summary>
    /// List of all symbol names (variables) in the script
    /// </summary>
    public string[] Symbols { get; set; } = Array.Empty<string>();

    /// <summary>
    /// List of input symbol names (variables with UI widgets)
    /// </summary>
    public string[] InputSymbols { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Indicates if the interpretation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if interpretation failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
