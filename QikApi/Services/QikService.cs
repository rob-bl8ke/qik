using CygSoft.Qik;
using CygSoft.Qik.Functions;
using Qik.UiWidgets;
using QikApi.Models;

namespace QikApi.Services;

/// <summary>
/// Implementation of the Qik service for script interpretation and processing
/// </summary>
public class QikService : IQikService
{
    private readonly IInterpreter _interpreter;
    private readonly IFunctionFactory _functionFactory;
    private readonly UiWidgetFactory _widgetFactory;

    public QikService()
    {
        _interpreter = new Interpreter();
        _functionFactory = new FunctionFactory(new PluginLoader());
        _widgetFactory = new UiWidgetFactory();
    }

    public InterpretResponse Interpret(InterpretRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Script))
            {
                return new InterpretResponse
                {
                    Success = false,
                    ErrorMessage = "Script cannot be empty"
                };
            }

            var terminal = _interpreter.Interpret(_functionFactory, request.Script);

            // Apply any provided variable values
            if (request.Variables != null)
            {
                foreach (var kvp in request.Variables)
                {
                    terminal.SetValue(kvp.Key, kvp.Value);
                }
            }

            // Get all values
            var values = new Dictionary<string, string>();
            foreach (var symbol in terminal.Symbols)
            {
                values[symbol] = terminal.GetValue(symbol);
            }

            return new InterpretResponse
            {
                Success = true,
                Values = values,
                Symbols = terminal.Symbols,
                InputSymbols = terminal.InputSymbols
            };
        }
        catch (Exception ex)
        {
            return new InterpretResponse
            {
                Success = false,
                ErrorMessage = $"Interpretation error: {ex.Message}"
            };
        }
    }

    public GetWidgetsResponse GetWidgets(GetWidgetsRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Script))
            {
                return new GetWidgetsResponse
                {
                    Success = false,
                    ErrorMessage = "Script cannot be empty"
                };
            }

            var widgets = _widgetFactory.BuildFromScript(request.Script);
            
            // Also interpret the script to get default values
            var terminal = _interpreter.Interpret(_functionFactory, request.Script);

            var widgetDtos = widgets.Select(w => new UiWidgetDto
            {
                VariableName = w.Symbol,
                Title = w.Title,
                Type = w.Type,
                DefaultValue = terminal.GetValue(w.Symbol)
            }).ToList();

            return new GetWidgetsResponse
            {
                Success = true,
                Widgets = widgetDtos
            };
        }
        catch (Exception ex)
        {
            return new GetWidgetsResponse
            {
                Success = false,
                ErrorMessage = $"Widget extraction error: {ex.Message}"
            };
        }
    }

    public EvaluateExpressionResponse EvaluateExpression(EvaluateExpressionRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Expression))
            {
                return new EvaluateExpressionResponse
                {
                    Success = false,
                    ErrorMessage = "Expression cannot be empty"
                };
            }

            // Build a script with context variables and the expression
            var scriptBuilder = new System.Text.StringBuilder();
            
            if (request.Context != null)
            {
                foreach (var kvp in request.Context)
                {
                    scriptBuilder.AppendLine($"{kvp.Key} => \"{kvp.Value}\";");
                }
            }

            scriptBuilder.AppendLine($"@result => {request.Expression};");

            var terminal = _interpreter.Interpret(_functionFactory, scriptBuilder.ToString());
            var result = terminal.GetValue("@result");

            return new EvaluateExpressionResponse
            {
                Success = true,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new EvaluateExpressionResponse
            {
                Success = false,
                ErrorMessage = $"Evaluation error: {ex.Message}"
            };
        }
    }

    public string GetValue(string script, string variableName)
    {
        try
        {
            var terminal = _interpreter.Interpret(_functionFactory, script);
            return terminal.GetValue(variableName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error getting value: {ex.Message}", ex);
        }
    }

    public List<FunctionInfoDto> GetAvailableFunctions()
    {
        return new List<FunctionInfoDto>
        {
            new() { Name = "abbreviate", Category = "Text Transformation", Signature = "abbreviate(text)", Description = "Abbreviates text by extracting the first letter of each word", Example = "abbreviate(\"Hello World\") // \"HW\"" },
            new() { Name = "base64Decode", Category = "Encoding", Signature = "base64Decode(text)", Description = "Decodes text from Base64 format", Example = "base64Decode(\"UmVkIFNveA==\") // \"Red Sox\"" },
            new() { Name = "base64Encode", Category = "Encoding", Signature = "base64Encode(text)", Description = "Encodes text to Base64 format", Example = "base64Encode(\"Red Sox\") // \"UmVkIFNveA==\"" },
            new() { Name = "camelCase", Category = "Text Transformation", Signature = "camelCase(text)", Description = "Converts text to camelCase format", Example = "camelCase(\"Hello World\") // \"helloWorld\"" },
            new() { Name = "currentDate", Category = "Generation", Signature = "currentDate([format])", Description = "Returns the current date with optional formatting", Example = "currentDate(\"yyyy-MM-dd\") // \"2025-10-16\"" },
            new() { Name = "doubleQuotes", Category = "Formatting", Signature = "doubleQuotes(text)", Description = "Surrounds text with double quotes", Example = "doubleQuotes(\"hello\") // \"\\\"hello\\\"\"" },
            new() { Name = "guid", Category = "Generation", Signature = "guid([case])", Description = "Generates a random GUID", Example = "guid(\"u\") // \"A3BB189E-8BF9-3888-9912-ACE4E6543002\"" },
            new() { Name = "htmlDecode", Category = "Encoding", Signature = "htmlDecode(text)", Description = "Decodes HTML-encoded text", Example = "htmlDecode(\"&lt;div&gt;\") // \"<div>\"" },
            new() { Name = "htmlEncode", Category = "Encoding", Signature = "htmlEncode(text)", Description = "Encodes text for safe HTML display", Example = "htmlEncode(\"<div>\") // \"&lt;div&gt;\"" },
            new() { Name = "indentLine", Category = "Formatting", Signature = "indentLine(text, indentType, count)", Description = "Indents text by specified number of tabs or spaces", Example = "indentLine(\"code\", TAB, 1) // \"\\tcode\"" },
            new() { Name = "lowerCase", Category = "Text Transformation", Signature = "lowerCase(text)", Description = "Converts text to lowercase", Example = "lowerCase(\"HELLO\") // \"hello\"" },
            new() { Name = "padLeft", Category = "Formatting", Signature = "padLeft(text, padChar, totalLength)", Description = "Pads text on the left with specified character", Example = "padLeft(\"12\", \"0\", 5) // \"00012\"" },
            new() { Name = "padRight", Category = "Formatting", Signature = "padRight(text, padChar, totalLength)", Description = "Pads text on the right with specified character", Example = "padRight(\"12\", \"0\", 5) // \"12000\"" },
            new() { Name = "properCase", Category = "Text Transformation", Signature = "properCase(text)", Description = "Converts text to Proper Case (Title Case)", Example = "properCase(\"hello world\") // \"Hello World\"" },
            new() { Name = "removePunctuation", Category = "Text Transformation", Signature = "removePunctuation(text)", Description = "Removes all punctuation characters", Example = "removePunctuation(\"Hello!\") // \"Hello\"" },
            new() { Name = "removeSpaces", Category = "Text Transformation", Signature = "removeSpaces(text)", Description = "Removes all whitespace characters", Example = "removeSpaces(\"Hello World\") // \"HelloWorld\"" },
            new() { Name = "replace", Category = "Text Transformation", Signature = "replace(text, findText, replaceText)", Description = "Replaces all occurrences of a substring", Example = "replace(\"Hello World\", \" \", \"_\") // \"Hello_World\"" },
            new() { Name = "upperCase", Category = "Text Transformation", Signature = "upperCase(text)", Description = "Converts text to uppercase", Example = "upperCase(\"hello\") // \"HELLO\"" },
            new() { Name = "urlDecode", Category = "Encoding", Signature = "urlDecode(text)", Description = "Decodes URL-encoded text", Example = "urlDecode(\"hello%20world\") // \"hello world\"" },
            new() { Name = "urlEncode", Category = "Encoding", Signature = "urlEncode(text)", Description = "Encodes text for safe URL use", Example = "urlEncode(\"hello world\") // \"hello%20world\"" }
        };
    }

    public GenerateResponse Generate(GenerateRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Script))
            {
                return new GenerateResponse
                {
                    Success = false,
                    ErrorMessage = "Script cannot be empty"
                };
            }

            // Decode the base64 script
            string decodedScript;
            try
            {
                var scriptBytes = Convert.FromBase64String(request.Script);
                decodedScript = System.Text.Encoding.UTF8.GetString(scriptBytes);
            }
            catch (Exception ex)
            {
                return new GenerateResponse
                {
                    Success = false,
                    ErrorMessage = $"Invalid base64 script: {ex.Message}"
                };
            }

            // Interpret the script to get terminal with all variables
            var terminal = _interpreter.Interpret(_functionFactory, decodedScript);

            // Apply input variables if provided
            if (request.Inputs != null && request.Inputs.Any())
            {
                foreach (var input in request.Inputs)
                {
                    try
                    {
                        terminal.SetValue(input.Key, input.Value);
                    }
                    catch (KeyNotFoundException)
                    {
                        // Input key doesn't exist as an InputSymbol in the script
                        continue;
                    }
                }
            }

            // Step 1: Process fragments - decode base64 content and replace placeholders with script values
            var processedFragments = new Dictionary<string, string>();
            if (request.Fragments != null && request.Fragments.Any())
            {
                foreach (var fragment in request.Fragments)
                {
                    try
                    {
                        // Decode base64 fragment content
                        var fragmentBytes = Convert.FromBase64String(fragment.Value);
                        var fragmentContent = System.Text.Encoding.UTF8.GetString(fragmentBytes);

                        // Replace placeholders with values from the interpreted script
                        var processedContent = ReplacePlaceholders(fragmentContent, terminal, request.PlaceholderPrefix, request.PlaceholderSuffix);
                        processedFragments[fragment.Key] = processedContent;
                    }
                    catch (Exception ex)
                    {
                        return new GenerateResponse
                        {
                            Success = false,
                            ErrorMessage = $"Error processing fragment '{fragment.Key}': {ex.Message}"
                        };
                    }
                }
            }

            // Step 2: Build documents by combining fragments
            var resultDocuments = new Dictionary<string, string>();
            if (request.Documents != null && request.Documents.Any())
            {
                foreach (var document in request.Documents)
                {
                    try
                    {
                        // The document value contains fragment keys that need to be resolved
                        var documentContent = BuildDocumentFromFragments(document.Value, processedFragments);
                        
                        // Encode the document content as Base64
                        var contentBytes = System.Text.Encoding.UTF8.GetBytes(documentContent);
                        var base64Content = Convert.ToBase64String(contentBytes);
                        
                        resultDocuments[document.Key] = base64Content;
                    }
                    catch (Exception ex)
                    {
                        return new GenerateResponse
                        {
                            Success = false,
                            ErrorMessage = $"Error building document '{document.Key}': {ex.Message}"
                        };
                    }
                }
            }

            return new GenerateResponse
            {
                Success = true,
                Documents = resultDocuments,
                Metadata = new Dictionary<string, object>
                {
                    { "fragmentCount", request.Fragments?.Count ?? 0 },
                    { "documentCount", resultDocuments.Count },
                    { "inputCount", request.Inputs?.Count ?? 0 },
                    { "symbolCount", terminal.Symbols?.Length ?? 0 }
                }
            };
        }
        catch (Exception ex)
        {
            return new GenerateResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private string ReplacePlaceholders(string content, ISymbolTerminal terminal, string prefix, string suffix)
    {
        var result = content;
        
        // Replace all symbols found in the terminal using configurable prefix/suffix format
        foreach (var symbol in terminal.Symbols)
        {
            var value = terminal.GetValue(symbol);
            if (value != null)
            {
                // Replace {prefix}symbolName{suffix} pattern
                var symbolWithoutAt = symbol.StartsWith("@") ? symbol.Substring(1) : symbol;
                result = result.Replace(prefix + symbolWithoutAt + suffix, value);
                
                // Also replace {prefix}@symbolName{suffix} pattern in case the symbol already has @ prefix
                result = result.Replace(prefix + symbol + suffix, value);
            }
        }

        return result;
    }

    private string BuildDocumentFromFragments(string documentTemplate, Dictionary<string, string> fragments)
    {
        var result = documentTemplate;

        // The document template contains fragment keys that need to be replaced with fragment content
        foreach (var fragment in fragments)
        {
            // Replace fragment placeholders in various formats
            result = result.Replace("{" + fragment.Key + "}", fragment.Value);
            result = result.Replace("{{" + fragment.Key + "}}", fragment.Value);
            result = result.Replace(fragment.Key, fragment.Value);
        }

        return result;
    }
}
