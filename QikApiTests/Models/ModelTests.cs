using FluentAssertions;
using QikApi.Models;

namespace QikApiTests.Models;

[TestFixture]
public class InterpretRequestTests
{
    [Test]
    public void InterpretRequest_CanBeInstantiated()
    {
        // Act
        var request = new InterpretRequest();

        // Assert
        request.Should().NotBeNull();
        request.Script.Should().NotBeNull();
    }

    [Test]
    public void InterpretRequest_ScriptProperty_CanBeSet()
    {
        // Arrange
        var request = new InterpretRequest();
        var script = "@test => \"value\";";

        // Act
        request.Script = script;

        // Assert
        request.Script.Should().Be(script);
    }

    [Test]
    public void InterpretRequest_VariablesProperty_CanBeNull()
    {
        // Arrange
        var request = new InterpretRequest();

        // Assert
        request.Variables.Should().BeNull();
    }

    [Test]
    public void InterpretRequest_VariablesProperty_CanBeSet()
    {
        // Arrange
        var request = new InterpretRequest();
        var variables = new Dictionary<string, string> { ["@test"] = "value" };

        // Act
        request.Variables = variables;

        // Assert
        request.Variables.Should().BeEquivalentTo(variables);
    }
}

[TestFixture]
public class InterpretResponseTests
{
    [Test]
    public void InterpretResponse_DefaultValues_AreInitialized()
    {
        // Act
        var response = new InterpretResponse();

        // Assert
        response.Values.Should().NotBeNull().And.BeEmpty();
        response.Symbols.Should().NotBeNull().And.BeEmpty();
        response.InputSymbols.Should().NotBeNull().And.BeEmpty();
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().BeNull();
    }

    [Test]
    public void InterpretResponse_CanSetAllProperties()
    {
        // Arrange
        var response = new InterpretResponse();

        // Act
        response.Success = true;
        response.Values = new Dictionary<string, string> { ["@test"] = "value" };
        response.Symbols = new[] { "@test" };
        response.InputSymbols = new[] { "@input" };
        response.ErrorMessage = "Error";

        // Assert
        response.Success.Should().BeTrue();
        response.Values.Should().ContainKey("@test");
        response.Symbols.Should().Contain("@test");
        response.InputSymbols.Should().Contain("@input");
        response.ErrorMessage.Should().Be("Error");
    }
}

[TestFixture]
public class GetWidgetsRequestTests
{
    [Test]
    public void GetWidgetsRequest_CanBeInstantiated()
    {
        // Act
        var request = new GetWidgetsRequest();

        // Assert
        request.Should().NotBeNull();
        request.Script.Should().NotBeNull();
    }

    [Test]
    public void GetWidgetsRequest_ScriptProperty_CanBeSet()
    {
        // Arrange
        var request = new GetWidgetsRequest();
        var script = "[title=\"Test\", type=\"text\"] @test => \"\";";

        // Act
        request.Script = script;

        // Assert
        request.Script.Should().Be(script);
    }
}

[TestFixture]
public class GetWidgetsResponseTests
{
    [Test]
    public void GetWidgetsResponse_DefaultValues_AreInitialized()
    {
        // Act
        var response = new GetWidgetsResponse();

        // Assert
        response.Widgets.Should().NotBeNull().And.BeEmpty();
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().BeNull();
    }

    [Test]
    public void GetWidgetsResponse_CanSetAllProperties()
    {
        // Arrange
        var response = new GetWidgetsResponse();
        var widgets = new List<UiWidgetDto>
        {
            new() { VariableName = "@test", Title = "Test", Type = "text" }
        };

        // Act
        response.Success = true;
        response.Widgets = widgets;
        response.ErrorMessage = "Error";

        // Assert
        response.Success.Should().BeTrue();
        response.Widgets.Should().HaveCount(1);
        response.ErrorMessage.Should().Be("Error");
    }
}

[TestFixture]
public class UiWidgetDtoTests
{
    [Test]
    public void UiWidgetDto_DefaultValues_AreInitialized()
    {
        // Act
        var widget = new UiWidgetDto();

        // Assert
        widget.VariableName.Should().NotBeNull();
        widget.Title.Should().NotBeNull();
        widget.Type.Should().NotBeNull();
        widget.DefaultValue.Should().NotBeNull();
    }

    [Test]
    public void UiWidgetDto_CanSetAllProperties()
    {
        // Arrange
        var widget = new UiWidgetDto();

        // Act
        widget.VariableName = "@name";
        widget.Title = "User Name";
        widget.Type = "text";
        widget.DefaultValue = "Guest";

        // Assert
        widget.VariableName.Should().Be("@name");
        widget.Title.Should().Be("User Name");
        widget.Type.Should().Be("text");
        widget.DefaultValue.Should().Be("Guest");
    }
}

[TestFixture]
public class EvaluateExpressionRequestTests
{
    [Test]
    public void EvaluateExpressionRequest_CanBeInstantiated()
    {
        // Act
        var request = new EvaluateExpressionRequest();

        // Assert
        request.Should().NotBeNull();
        request.Expression.Should().NotBeNull();
    }

    [Test]
    public void EvaluateExpressionRequest_ExpressionProperty_CanBeSet()
    {
        // Arrange
        var request = new EvaluateExpressionRequest();
        var expression = "upperCase(@name)";

        // Act
        request.Expression = expression;

        // Assert
        request.Expression.Should().Be(expression);
    }

    [Test]
    public void EvaluateExpressionRequest_ContextProperty_CanBeNull()
    {
        // Arrange
        var request = new EvaluateExpressionRequest();

        // Assert
        request.Context.Should().BeNull();
    }

    [Test]
    public void EvaluateExpressionRequest_ContextProperty_CanBeSet()
    {
        // Arrange
        var request = new EvaluateExpressionRequest();
        var context = new Dictionary<string, string> { ["@name"] = "John" };

        // Act
        request.Context = context;

        // Assert
        request.Context.Should().BeEquivalentTo(context);
    }
}

[TestFixture]
public class EvaluateExpressionResponseTests
{
    [Test]
    public void EvaluateExpressionResponse_DefaultValues_AreInitialized()
    {
        // Act
        var response = new EvaluateExpressionResponse();

        // Assert
        response.Result.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().BeNull();
    }

    [Test]
    public void EvaluateExpressionResponse_CanSetAllProperties()
    {
        // Arrange
        var response = new EvaluateExpressionResponse();

        // Act
        response.Success = true;
        response.Result = "HELLO";
        response.ErrorMessage = "Error";

        // Assert
        response.Success.Should().BeTrue();
        response.Result.Should().Be("HELLO");
        response.ErrorMessage.Should().Be("Error");
    }
}

[TestFixture]
public class FunctionInfoDtoTests
{
    [Test]
    public void FunctionInfoDto_DefaultValues_AreInitialized()
    {
        // Act
        var function = new FunctionInfoDto();

        // Assert
        function.Name.Should().NotBeNull();
        function.Description.Should().NotBeNull();
        function.Signature.Should().NotBeNull();
        function.Example.Should().NotBeNull();
        function.Category.Should().NotBeNull();
    }

    [Test]
    public void FunctionInfoDto_CanSetAllProperties()
    {
        // Arrange
        var function = new FunctionInfoDto();

        // Act
        function.Name = "upperCase";
        function.Description = "Converts text to uppercase";
        function.Signature = "upperCase(text)";
        function.Example = "upperCase(\"hello\")";
        function.Category = "Text Transformation";

        // Assert
        function.Name.Should().Be("upperCase");
        function.Description.Should().Be("Converts text to uppercase");
        function.Signature.Should().Be("upperCase(text)");
        function.Example.Should().Be("upperCase(\"hello\")");
        function.Category.Should().Be("Text Transformation");
    }
}
