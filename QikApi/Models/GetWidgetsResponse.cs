namespace QikApi.Models;

/// <summary>
/// Response model containing extracted UI widgets
/// </summary>
public class GetWidgetsResponse
{
    /// <summary>
    /// List of UI widgets extracted from the script
    /// </summary>
    public List<UiWidgetDto> Widgets { get; set; } = new();

    /// <summary>
    /// Indicates if the extraction was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if extraction failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
