namespace QikApi.Models;

/// <summary>
/// Response model for expression evaluation
/// </summary>
public class EvaluateExpressionResponse
{
    /// <summary>
    /// The result of the expression evaluation
    /// </summary>
    public string Result { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the evaluation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if evaluation failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
