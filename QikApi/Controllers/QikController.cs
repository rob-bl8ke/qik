using Microsoft.AspNetCore.Mvc;
using QikApi.Models;
using QikApi.Services;

namespace QikApi.Controllers;

/// <summary>
/// Controller for Qik script interpretation operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class QikController : ControllerBase
{
    private readonly IQikService _qikService;
    private readonly ILogger<QikController> _logger;

    public QikController(IQikService qikService, ILogger<QikController> logger)
    {
        _qikService = qikService;
        _logger = logger;
    }

    /// <summary>
    /// Interprets a Qik script and returns all variable values
    /// </summary>
    /// <param name="request">The interpretation request containing the script</param>
    /// <returns>Interpretation results with all variable values</returns>
    /// <response code="200">Returns the interpretation results</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost("interpret")]
    [ProducesResponseType(typeof(InterpretResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<InterpretResponse> Interpret([FromBody] InterpretRequest request)
    {
        _logger.LogInformation("Interpreting Qik script");

        if (string.IsNullOrWhiteSpace(request.Script))
        {
            return BadRequest(new { error = "Script cannot be empty" });
        }

        var response = _qikService.Interpret(request);

        if (!response.Success)
        {
            _logger.LogWarning("Interpretation failed: {ErrorMessage}", response.ErrorMessage);
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Extracts UI widgets from a Qik script
    /// </summary>
    /// <param name="request">The request containing the script</param>
    /// <returns>List of UI widgets found in the script</returns>
    /// <response code="200">Returns the extracted UI widgets</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost("widgets")]
    [ProducesResponseType(typeof(GetWidgetsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<GetWidgetsResponse> GetWidgets([FromBody] GetWidgetsRequest request)
    {
        _logger.LogInformation("Extracting UI widgets from Qik script");

        if (string.IsNullOrWhiteSpace(request.Script))
        {
            return BadRequest(new { error = "Script cannot be empty" });
        }

        var response = _qikService.GetWidgets(request);

        if (!response.Success)
        {
            _logger.LogWarning("Widget extraction failed: {ErrorMessage}", response.ErrorMessage);
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Evaluates a single expression with optional context variables
    /// </summary>
    /// <param name="request">The expression and context to evaluate</param>
    /// <returns>The evaluation result</returns>
    /// <response code="200">Returns the evaluation result</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(EvaluateExpressionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EvaluateExpressionResponse> EvaluateExpression([FromBody] EvaluateExpressionRequest request)
    {
        _logger.LogInformation("Evaluating expression: {Expression}", request.Expression);

        if (string.IsNullOrWhiteSpace(request.Expression))
        {
            return BadRequest(new { error = "Expression cannot be empty" });
        }

        var response = _qikService.EvaluateExpression(request);

        if (!response.Success)
        {
            _logger.LogWarning("Expression evaluation failed: {ErrorMessage}", response.ErrorMessage);
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Gets information about all available Qik functions
    /// </summary>
    /// <returns>List of available functions with their documentation</returns>
    /// <response code="200">Returns the list of available functions</response>
    [HttpGet("functions")]
    [ProducesResponseType(typeof(List<FunctionInfoDto>), StatusCodes.Status200OK)]
    public ActionResult<List<FunctionInfoDto>> GetFunctions()
    {
        _logger.LogInformation("Retrieving available functions");
        var functions = _qikService.GetAvailableFunctions();
        return Ok(functions);
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns>API health status</returns>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetHealth()
    {
        return Ok(new { status = "healthy", service = "Qik API", version = "1.0.0" });
    }
}
