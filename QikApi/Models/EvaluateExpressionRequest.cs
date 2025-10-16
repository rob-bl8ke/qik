namespace QikApi.Models;

/// <summary>
/// Request model for evaluating a single expression
/// </summary>
public class EvaluateExpressionRequest
{
    /// <summary>
    /// The expression to evaluate (can include variables and functions)
    /// </summary>
    public string Expression { get; set; } = string.Empty;

    /// <summary>
    /// Optional context variables for the expression
    /// </summary>
    public Dictionary<string, string>? Context { get; set; }
}
