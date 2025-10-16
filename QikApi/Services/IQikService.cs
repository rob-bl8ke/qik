using QikApi.Models;

namespace QikApi.Services;

/// <summary>
/// Service interface for Qik script interpretation and processing
/// </summary>
public interface IQikService
{
    /// <summary>
    /// Interprets a Qik script and returns all variable values
    /// </summary>
    InterpretResponse Interpret(InterpretRequest request);

    /// <summary>
    /// Extracts UI widgets from a Qik script
    /// </summary>
    GetWidgetsResponse GetWidgets(GetWidgetsRequest request);

    /// <summary>
    /// Evaluates a single expression with optional context
    /// </summary>
    EvaluateExpressionResponse EvaluateExpression(EvaluateExpressionRequest request);

    /// <summary>
    /// Gets a specific variable value from an interpreted script
    /// </summary>
    string GetValue(string script, string variableName);

    /// <summary>
    /// Gets information about all available functions
    /// </summary>
    List<FunctionInfoDto> GetAvailableFunctions();
}
