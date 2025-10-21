namespace QikApi.Models;

/// <summary>
/// Represents a UI widget extracted from a Qik script
/// </summary>
public class UiWidgetDto
{
    /// <summary>
    /// The variable name associated with this widget
    /// </summary>
    public string VariableName { get; set; } = string.Empty;

    /// <summary>
    /// The display title for the input field
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The input type (e.g., "text", "number", "email")
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The default value for this widget
    /// </summary>
    public string DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// Placeholder text to display in the input field
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Indicates whether this field is required
    /// </summary>
    public bool? Required { get; set; }

    /// <summary>
    /// List of options for select/dropdown widgets
    /// </summary>
    public string[]? Options { get; set; }
}
