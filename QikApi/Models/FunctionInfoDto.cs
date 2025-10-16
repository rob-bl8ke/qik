namespace QikApi.Models;

/// <summary>
/// Information about an available Qik function
/// </summary>
public class FunctionInfoDto
{
    /// <summary>
    /// The function name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of what the function does
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Function signature showing parameters
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// Example usage of the function
    /// </summary>
    public string Example { get; set; } = string.Empty;

    /// <summary>
    /// Category of the function (e.g., "Text Transformation", "Encoding")
    /// </summary>
    public string Category { get; set; } = string.Empty;
}
