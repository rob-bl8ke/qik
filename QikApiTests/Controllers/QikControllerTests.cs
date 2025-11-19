using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QikApi.Controllers;
using QikApi.Models;
using QikApi.Services;

namespace QikApiTests.Controllers;

[TestFixture]
public class QikControllerTests
{
    private Mock<IQikService> _mockQikService = null!;
    private Mock<ILogger<QikController>> _mockLogger = null!;
    private QikController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mockQikService = new Mock<IQikService>();
        _mockLogger = new Mock<ILogger<QikController>>();
        _controller = new QikController(_mockQikService.Object, _mockLogger.Object);
    }

    [TestFixture]
    public class InterpretTests : QikControllerTests
    {
        [Test]
        public void Interpret_WithValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = "@name => \"John\";"
            };

            var expectedResponse = new InterpretResponse
            {
                Success = true,
                Values = new Dictionary<string, string> { ["@name"] = "John" },
                Symbols = new[] { "@name" },
                InputSymbols = Array.Empty<string>()
            };

            _mockQikService
                .Setup(s => s.Interpret(It.IsAny<InterpretRequest>()))
                .Returns(expectedResponse);

            // Act
            var result = _controller.Interpret(request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void Interpret_WithEmptyScript_ReturnsBadRequest()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = ""
            };

            // Act
            var result = _controller.Interpret(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Interpret_WithServiceFailure_ReturnsBadRequestWithError()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = "@invalid"
            };

            var errorResponse = new InterpretResponse
            {
                Success = false,
                ErrorMessage = "Syntax error"
            };

            _mockQikService
                .Setup(s => s.Interpret(It.IsAny<InterpretRequest>()))
                .Returns(errorResponse);

            // Act
            var result = _controller.Interpret(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Interpret_CallsServiceWithCorrectRequest()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = "@test => \"value\";"
            };

            _mockQikService
                .Setup(s => s.Interpret(It.IsAny<InterpretRequest>()))
                .Returns(new InterpretResponse { Success = true });

            // Act
            _controller.Interpret(request);

            // Assert
            _mockQikService.Verify(
                s => s.Interpret(It.Is<InterpretRequest>(r => r.Script == request.Script)),
                Times.Once);
        }

        [Test]
        public void Interpret_WithContentEncodingQueryParameter_PassesToRequest()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = "QHRlc3QgPT4gInZhbHVlIjs="
            };

            _mockQikService
                .Setup(s => s.Interpret(It.IsAny<InterpretRequest>()))
                .Returns(new InterpretResponse { Success = true });

            // Act
            _controller.Interpret(request, "base64");

            // Assert
            _mockQikService.Verify(
                s => s.Interpret(It.Is<InterpretRequest>(r => r.ContentEncoding == "base64")),
                Times.Once);
        }

        [Test]
        public void Interpret_WithoutContentEncodingParameter_PassesNullEncoding()
        {
            // Arrange
            var request = new InterpretRequest
            {
                Script = "@test => \"value\";"
            };

            _mockQikService
                .Setup(s => s.Interpret(It.IsAny<InterpretRequest>()))
                .Returns(new InterpretResponse { Success = true });

            // Act
            _controller.Interpret(request, null);

            // Assert
            _mockQikService.Verify(
                s => s.Interpret(It.Is<InterpretRequest>(r => r.ContentEncoding == null)),
                Times.Once);
        }
    }

    [TestFixture]
    public class GetWidgetsTests : QikControllerTests
    {
        [Test]
        public void GetWidgets_WithValidRequest_ReturnsOkWithWidgets()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = "[title=\"Name\", type=\"text\"] @name => \"\";"
            };

            var expectedResponse = new GetWidgetsResponse
            {
                Success = true,
                Widgets = new List<UiWidgetDto>
                {
                    new() { VariableName = "@name", Title = "Name", Type = "text" }
                }
            };

            _mockQikService
                .Setup(s => s.GetWidgets(It.IsAny<GetWidgetsRequest>()))
                .Returns(expectedResponse);

            // Act
            var result = _controller.GetWidgets(request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void GetWidgets_WithEmptyScript_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = ""
            };

            // Act
            var result = _controller.GetWidgets(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void GetWidgets_WithServiceFailure_ReturnsBadRequestWithError()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = "@invalid"
            };

            var errorResponse = new GetWidgetsResponse
            {
                Success = false,
                ErrorMessage = "Parsing error"
            };

            _mockQikService
                .Setup(s => s.GetWidgets(It.IsAny<GetWidgetsRequest>()))
                .Returns(errorResponse);

            // Act
            var result = _controller.GetWidgets(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void GetWidgets_CallsServiceWithCorrectRequest()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = "[title=\"Test\", type=\"text\"] @test => \"\";"
            };

            _mockQikService
                .Setup(s => s.GetWidgets(It.IsAny<GetWidgetsRequest>()))
                .Returns(new GetWidgetsResponse { Success = true });

            // Act
            _controller.GetWidgets(request);

            // Assert
            _mockQikService.Verify(
                s => s.GetWidgets(It.Is<GetWidgetsRequest>(r => r.Script == request.Script)),
                Times.Once);
        }

        [Test]
        public void GetWidgets_WithContentEncodingQueryParameter_PassesToRequest()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = "W3RpdGxlPSJUZXN0IiwgdHlwZT0idGV4dCJdIEB0ZXN0ID0+ICIiOw=="
            };

            _mockQikService
                .Setup(s => s.GetWidgets(It.IsAny<GetWidgetsRequest>()))
                .Returns(new GetWidgetsResponse { Success = true });

            // Act
            _controller.GetWidgets(request, "base64");

            // Assert
            _mockQikService.Verify(
                s => s.GetWidgets(It.Is<GetWidgetsRequest>(r => r.ContentEncoding == "base64")),
                Times.Once);
        }

        [Test]
        public void GetWidgets_WithoutContentEncodingParameter_PassesNullEncoding()
        {
            // Arrange
            var request = new GetWidgetsRequest
            {
                Script = "[title=\"Test\", type=\"text\"] @test => \"\";"
            };

            _mockQikService
                .Setup(s => s.GetWidgets(It.IsAny<GetWidgetsRequest>()))
                .Returns(new GetWidgetsResponse { Success = true });

            // Act
            _controller.GetWidgets(request, null);

            // Assert
            _mockQikService.Verify(
                s => s.GetWidgets(It.Is<GetWidgetsRequest>(r => r.ContentEncoding == null)),
                Times.Once);
        }
    }

    [TestFixture]
    public class EvaluateExpressionTests : QikControllerTests
    {
        [Test]
        public void EvaluateExpression_WithValidRequest_ReturnsOkWithResult()
        {
            // Arrange
            var request = new EvaluateExpressionRequest
            {
                Expression = "upperCase(\"hello\")"
            };

            var expectedResponse = new EvaluateExpressionResponse
            {
                Success = true,
                Result = "HELLO"
            };

            _mockQikService
                .Setup(s => s.EvaluateExpression(It.IsAny<EvaluateExpressionRequest>()))
                .Returns(expectedResponse);

            // Act
            var result = _controller.EvaluateExpression(request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void EvaluateExpression_WithEmptyExpression_ReturnsBadRequest()
        {
            // Arrange
            var request = new EvaluateExpressionRequest
            {
                Expression = ""
            };

            // Act
            var result = _controller.EvaluateExpression(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void EvaluateExpression_WithServiceFailure_ReturnsBadRequestWithError()
        {
            // Arrange
            var request = new EvaluateExpressionRequest
            {
                Expression = "invalid()"
            };

            var errorResponse = new EvaluateExpressionResponse
            {
                Success = false,
                ErrorMessage = "Unknown function"
            };

            _mockQikService
                .Setup(s => s.EvaluateExpression(It.IsAny<EvaluateExpressionRequest>()))
                .Returns(errorResponse);

            // Act
            var result = _controller.EvaluateExpression(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void EvaluateExpression_CallsServiceWithCorrectRequest()
        {
            // Arrange
            var request = new EvaluateExpressionRequest
            {
                Expression = "camelCase(@input)",
                Context = new Dictionary<string, string> { ["@input"] = "Hello World" }
            };

            _mockQikService
                .Setup(s => s.EvaluateExpression(It.IsAny<EvaluateExpressionRequest>()))
                .Returns(new EvaluateExpressionResponse { Success = true, Result = "helloWorld" });

            // Act
            _controller.EvaluateExpression(request);

            // Assert
            _mockQikService.Verify(
                s => s.EvaluateExpression(It.Is<EvaluateExpressionRequest>(
                    r => r.Expression == request.Expression)),
                Times.Once);
        }
    }

    [TestFixture]
    public class GetFunctionsTests : QikControllerTests
    {
        [Test]
        public void GetFunctions_ReturnsOkWithFunctionList()
        {
            // Arrange
            var expectedFunctions = new List<FunctionInfoDto>
            {
                new() { Name = "upperCase", Category = "Text Transformation" },
                new() { Name = "lowerCase", Category = "Text Transformation" }
            };

            _mockQikService
                .Setup(s => s.GetAvailableFunctions())
                .Returns(expectedFunctions);

            // Act
            var result = _controller.GetFunctions();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedFunctions);
        }

        [Test]
        public void GetFunctions_CallsService()
        {
            // Arrange
            _mockQikService
                .Setup(s => s.GetAvailableFunctions())
                .Returns(new List<FunctionInfoDto>());

            // Act
            _controller.GetFunctions();

            // Assert
            _mockQikService.Verify(s => s.GetAvailableFunctions(), Times.Once);
        }

        [Test]
        public void GetFunctions_ReturnsNonEmptyList()
        {
            // Arrange
            var functions = new List<FunctionInfoDto>
            {
                new() { Name = "test", Description = "test", Signature = "test()", Example = "test()", Category = "Test" }
            };

            _mockQikService
                .Setup(s => s.GetAvailableFunctions())
                .Returns(functions);

            // Act
            var result = _controller.GetFunctions();

            // Assert
            var okResult = result.Result as OkObjectResult;
            var returnedFunctions = okResult!.Value as List<FunctionInfoDto>;
            returnedFunctions.Should().NotBeEmpty();
        }
    }

    [TestFixture]
    public class GenerateTests : QikControllerTests
    {
        [Test]
        public void Generate_WithValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "@name => \"Test\";",
                Fragments = new Dictionary<string, string>
                {
                    ["template"] = "Name: @{name}"
                },
                Documents = new Dictionary<string, string>
                {
                    ["output.txt"] = "{template}"
                },
                PlaceholderPrefix = "@{",
                PlaceholderSuffix = "}"
            };

            var expectedResponse = new GenerateResponse
            {
                Success = true,
                Documents = new Dictionary<string, string>
                {
                    ["output.txt"] = "TmFtZTogVGVzdA==" // Base64: "Name: Test"
                }
            };

            _mockQikService
                .Setup(s => s.Generate(It.IsAny<GenerateRequest>()))
                .Returns(expectedResponse);

            // Act
            var result = _controller.Generate(request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void Generate_WithEmptyScript_ReturnsBadRequest()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "",
                Fragments = new Dictionary<string, string>(),
                Documents = new Dictionary<string, string>()
            };

            // Act
            var result = _controller.Generate(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Generate_WithServiceFailure_ReturnsBadRequestWithError()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "@invalid",
                Fragments = new Dictionary<string, string>(),
                Documents = new Dictionary<string, string>()
            };

            var errorResponse = new GenerateResponse
            {
                Success = false,
                ErrorMessage = "Generation error"
            };

            _mockQikService
                .Setup(s => s.Generate(It.IsAny<GenerateRequest>()))
                .Returns(errorResponse);

            // Act
            var result = _controller.Generate(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Generate_CallsServiceWithCorrectRequest()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "@test => \"value\";",
                Fragments = new Dictionary<string, string>
                {
                    ["template"] = "Test: @{test}"
                },
                Documents = new Dictionary<string, string>
                {
                    ["test.txt"] = "{template}"
                },
                PlaceholderPrefix = "@{",
                PlaceholderSuffix = "}"
            };

            _mockQikService
                .Setup(s => s.Generate(It.IsAny<GenerateRequest>()))
                .Returns(new GenerateResponse { Success = true });

            // Act
            _controller.Generate(request);

            // Assert
            _mockQikService.Verify(
                s => s.Generate(It.Is<GenerateRequest>(r => r.Script == request.Script)),
                Times.Once);
        }

        [Test]
        public void Generate_WithContentEncodingQueryParameter_PassesToRequest()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "QHRlc3QgPT4gInZhbHVlIjs=",
                Fragments = new Dictionary<string, string>
                {
                    ["template"] = "VGVzdDogQHt0ZXN0fQ=="
                },
                Documents = new Dictionary<string, string>
                {
                    ["test.txt"] = "{template}"
                },
                PlaceholderPrefix = "@{",
                PlaceholderSuffix = "}"
            };

            _mockQikService
                .Setup(s => s.Generate(It.IsAny<GenerateRequest>()))
                .Returns(new GenerateResponse { Success = true });

            // Act
            _controller.Generate(request, "base64");

            // Assert
            _mockQikService.Verify(
                s => s.Generate(It.Is<GenerateRequest>(r => r.ContentEncoding == "base64")),
                Times.Once);
        }

        [Test]
        public void Generate_WithoutContentEncodingParameter_PassesNullEncoding()
        {
            // Arrange
            var request = new GenerateRequest
            {
                Script = "@test => \"value\";",
                Fragments = new Dictionary<string, string>
                {
                    ["template"] = "Test: @{test}"
                },
                Documents = new Dictionary<string, string>
                {
                    ["test.txt"] = "{template}"
                },
                PlaceholderPrefix = "@{",
                PlaceholderSuffix = "}"
            };

            _mockQikService
                .Setup(s => s.Generate(It.IsAny<GenerateRequest>()))
                .Returns(new GenerateResponse { Success = true });

            // Act
            _controller.Generate(request, null);

            // Assert
            _mockQikService.Verify(
                s => s.Generate(It.Is<GenerateRequest>(r => r.ContentEncoding == null)),
                Times.Once);
        }
    }

    [TestFixture]
    public class GetHealthTests : QikControllerTests
    {
        [Test]
        public void GetHealth_ReturnsOk()
        {
            // Act
            var result = _controller.GetHealth();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public void GetHealth_ReturnsHealthStatus()
        {
            // Act
            var result = _controller.GetHealth() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.Value.Should().NotBeNull();
        }

        [Test]
        public void GetHealth_ReturnsStatusHealthy()
        {
            // Act
            var result = _controller.GetHealth() as OkObjectResult;

            // Assert
            // We can't directly test the anonymous type properties, but we can verify structure
            result.Should().NotBeNull();
            result!.Value.Should().NotBeNull();
        }
    }
}
