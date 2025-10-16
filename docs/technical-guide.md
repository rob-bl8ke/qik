# Qik Technical Guide

## Overview

Qik is a script-based template generation library built on ANTLR4 that provides a powerful syntax for defining variables, expressions, and conditional logic. It's designed to help developers generate dynamic text content with complex transformations and business logic.

**NuGet Package**: `rob_bl8ke.Qik`  
**Repository**: https://github.com/rob-bl8ke/Qik

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Available Functions](#available-functions)
3. [Core Concepts](#core-concepts)
4. [Syntax Reference](#syntax-reference)
5. [Conditional Logic](#conditional-logic)
6. [Advanced Features](#advanced-features)
7. [Examples](#examples)
8. [Working with the Interpreter](#working-with-the-interpreter)
9. [Function Reference](#function-reference)
10. [Best Practices](#best-practices)
11. [Error Handling](#error-handling)

---

## Getting Started

### Installation

```bash
dotnet add package rob_bl8ke.Qik
```

### Basic Usage

```csharp
using CygSoft.Qik;
using CygSoft.Qik.Functions;

// Create an interpreter instance
var interpreter = new Interpreter();

// Define your Qik script
var script = @"
    @name => ""John Doe"";
    @greeting => ""Hello, "" + @name + ""!"";
";

// Interpret the script
var terminal = interpreter.Interpret(new FunctionFactory(pluginLoader), script);

// Retrieve values
var greeting = terminal.GetValue("@greeting"); // "Hello, John Doe!"
```

---

## Available Functions

Qik provides a comprehensive set of built-in functions for text transformation, encoding, and generation. For detailed documentation on each function, see the [Function Reference](#function-reference) section.

### Quick Reference

**Text Transformation**: `camelCase`, `upperCase`, `lowerCase`, `properCase`, `replace`, `abbreviate`, `removePunctuation`, `removeSpaces`

**Padding & Formatting**: `padLeft`, `padRight`, `indentLine`, `doubleQuotes`

**Encoding**: `base64Encode`, `base64Decode`, `urlEncode`, `urlDecode`, `htmlEncode`, `htmlDecode`

**Generation**: `guid`, `currentDate`

### Function Composition

Functions can be nested and combined:

```
@result => upperCase(replace(@input, " ", "_"));
@formatted => doubleQuotes(camelCase(@className));
@encoded => base64Encode(urlEncode(@data));
```

---

## Core Concepts

### 1. Variables

All variables in Qik start with the `@` symbol and must be declared with the `=>` assignment operator.

**Syntax:**
```
@variableName => value;
```

**Example:**
```
@name => "Alice";
@age => "30";
```

### 2. Expressions

Expressions are derived values that can use functions, concatenation, or conditional logic.

**Syntax:**
```
@result => expression;
```

### 3. Comments

Qik supports both block and line comments:

```
/* This is a block comment */

// This is a line comment
@var => "value"; // Inline comment
```

---

## Syntax Reference

### Variable Declaration

#### Simple Assignment
```
@input => "Hello World";
```

#### With UI Widget Metadata
You can attach UI metadata to variables for form generation:

```
[title = "Enter Name", type = "text"] @userName => "Default Name";
```

**Supported UI Widget Properties:**
- `title` - Display label for the input
- `type` - Input type (e.g., "text", "number", etc.)

### String Concatenation

Use the `+` operator to concatenate strings:

```
@firstName => "John";
@lastName => "Doe";
@fullName => @firstName + " " + @lastName;
```

### Constants

Built-in constants for special characters:

- `TAB` - Tab character
- `SPACE` - Space character
- `NEWLINE` - Newline character

**Example:**
```
@code => "{" + NEWLINE + TAB + "return x;" + NEWLINE + "}";
```

---

## Conditional Logic

### 1. Ternary (IIF) Operator

**Syntax:**
```
@result => condition ? trueValue : falseValue;
```

**Examples:**
```
@status => @count == "0" ? "empty" : "not empty";

@message => @user == "admin" ? upperCase("ADMIN") : lowerCase("user");

@greeting => @timeOfDay == "morning" ? "Good morning" + " sir" : "Good evening" + " sir";
```

### 2. If-Else Statements

**Syntax:**
```
@result =>
    if condition then value
    else if condition then value
    else value
;
```

**Example:**
```
@color =>
    if @score == "A" then "green"
    else if @score == "B" then "yellow"
    else "red"
;
```

### 3. Switch Statements

**Syntax:**
```
@result =>
    switch expression
        case "value1" then result1
        case "value2" then result2
        else defaultResult
;
```

**Example:**
```
@dayType =>
    switch @day
        case "Saturday" then "Weekend"
        case "Sunday" then "Weekend"
        else "Weekday"
;
```

---

## Advanced Features

### Multi-line Code Generation

Generate complex formatted code structures:

```
@codeBlock =>
    "{" + NEWLINE +
        indentLine("int x = y;", TAB, 1) + NEWLINE +
        indentLine("if (x == 2)", TAB, 1) + NEWLINE +
        indentLine("return y;", TAB, 2) + NEWLINE +
        indentLine("else", TAB, 1) + NEWLINE +
        indentLine("return x;", TAB, 2) + NEWLINE +
    "}";
```

### Dynamic Variable References

Variables can reference other variables:

```
@entity => "EmailAttribute";
@camelEntity => camelCase(@entity);
@property => @camelEntity + "Id";  // "emailAttributeId"
```

### Complex Conditionals with Functions

```
@displayName =>
    camelCase(@entity) == "emailAttribute" 
        ? properCase("happy") 
        : properCase("sad");
```

### Comparison Operators

- `==` - Equals
- `!=` - Not equals

```
@isValid => @status != "error" ? "valid" : "invalid";
```

---

## Examples

### Example 1: User Greeting Generator

```
[title = "User Name", type = "text"] @userName => "Guest";
[title = "Time of Day", type = "text"] @timeOfDay => "morning";

@greeting =>
    if @timeOfDay == "morning" then "Good morning, " + @userName
    else if @timeOfDay == "afternoon" then "Good afternoon, " + @userName
    else "Good evening, " + @userName
;
```

### Example 2: Code Class Generator

```
@className => "UserService";
@namespace => "MyApp.Services";

@classFile =>
    "namespace " + @namespace + NEWLINE +
    "{" + NEWLINE +
        indentLine("public class " + @className, TAB, 1) + NEWLINE +
        indentLine("{", TAB, 1) + NEWLINE +
        indentLine("}", TAB, 1) + NEWLINE +
    "}";
```

### Example 3: API URL Builder

```
@baseUrl => "https://api.example.com";
@endpoint => "users/search";
@query => "john doe";

@encodedQuery => urlEncode(@query);
@fullUrl => @baseUrl + "/" + @endpoint + "?q=" + @encodedQuery;
```

### Example 4: Configuration Generator

```
@environment => "production";

@dbConnection =>
    switch @environment
        case "development" then "Server=localhost;Database=DevDB"
        case "staging" then "Server=staging.db;Database=StageDB"
        else "Server=prod.db;Database=ProdDB"
;

@logLevel =>
    @environment == "production" ? "Error" : "Debug";
```

### Example 5: HTML Template Generator

```
@title => "Welcome Page";
@content => "Hello World";

@html =>
    "<!DOCTYPE html>" + NEWLINE +
    "<html>" + NEWLINE +
        indentLine("<head>", TAB, 1) + NEWLINE +
        indentLine("<title>" + htmlEncode(@title) + "</title>", TAB, 2) + NEWLINE +
        indentLine("</head>", TAB, 1) + NEWLINE +
        indentLine("<body>", TAB, 1) + NEWLINE +
        indentLine("<p>" + htmlEncode(@content) + "</p>", TAB, 2) + NEWLINE +
        indentLine("</body>", TAB, 1) + NEWLINE +
    "</html>";
```

---

## Working with the Interpreter

### Basic Interpretation

```csharp
var interpreter = new Interpreter();
var terminal = interpreter.Interpret(functionFactory, script);
```

### Retrieving Values

```csharp
// Get a single value
string value = terminal.GetValue("@variableName");

// Get all symbols
string[] allSymbols = terminal.Symbols;

// Get input symbols only
string[] inputs = terminal.InputSymbols;
```

### Setting Values Dynamically

```csharp
// Update an input variable after interpretation
terminal.SetValue("@inputVar", "New Value");

// Re-retrieve updated value
string updated = terminal.GetValue("@inputVar");
```

### UI Widget Extraction

```csharp
using Qik.UiWidgets;

var widgetFactory = new UiWidgetFactory();
UiWidget[] widgets = widgetFactory.BuildFromScript(script);

foreach (var widget in widgets)
{
    Console.WriteLine($"Title: {widget.Title}");
    Console.WriteLine($"Type: {widget.Type}");
}
```

---

## Grammar Rules Summary

### Variable Declaration Pattern
```
inputDecl: ([uiWidget] VARIABLE '=>' STRING ';')
```

### Expression Declaration Pattern
```
funcDecl: VARIABLE '=>' (stat|switchExpr|ifExpr) ';'
```

### Function Call Pattern
```
func: IDENTIFIER ('(' funcArg (',' funcArg)* ')')
```

### Valid Identifiers
- Must start with a letter or underscore
- Can contain letters, numbers, and underscores
- Examples: `camelCase`, `base64Encode`, `replace_text`

---

## Function Reference

This section provides detailed documentation for all available functions.

### abbreviate(text)
**Description**: Abbreviates text by extracting the first letter of each word. Works with PascalCase, camelCase, snake_case, and space-separated words.

**Arguments**:
- `text` (Required) - The text expression to abbreviate

**Examples**:
```
abbreviate("Hello World")      // "HW"
abbreviate("hello world")      // "hw"
abbreviate("HelloWorld")       // "HW"
abbreviate("hello_world")      // "hw"
```

---

### base64Decode(text)
**Description**: Decodes text from Base64 format back to the original string.

**Arguments**:
- `text` (Required) - The Base64-encoded string to decode

**Example**:
```
@decoded => base64Decode("UmVkIFNveA==");  // "Red Sox"
```

---

### base64Encode(text)
**Description**: Encodes a text string to Base64 format, useful for transmitting data safely.

**Arguments**:
- `text` (Required) - The string to encode

**Example**:
```
@encoded => base64Encode("Red Sox");  // "UmVkIFNveA=="
```

---

### camelCase(text)
**Description**: Converts text to camelCase format - compound words with no spaces and an initial lowercase letter, with each remaining word starting with uppercase.

**Arguments**:
- `text` (Required) - The text to convert

**Examples**:
```
camelCase("Hello World")       // "helloWorld"
camelCase("email_address")     // "emailAddress"
camelCase("UserService")       // "userService"
```

---

### currentDate([format])
**Description**: Returns the current date, optionally formatted according to a specified pattern.

**Arguments**:
- `format` (Optional) - Date format string. Default is long date format.

**Examples**:
```
currentDate()                      // "October 16, 2025" (or system long format)
currentDate("dd/MM/yyyy")          // "16/10/2025"
currentDate("yyyy-MM-dd")          // "2025-10-16"
currentDate("MMMM dd, yyyy")       // "October 16, 2025"
```

Format follows [.NET DateTime format patterns](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

---

### doubleQuotes(text)
**Description**: Surrounds the text expression with double quotes.

**Arguments**:
- `text` (Required) - The text expression to wrap

**Example**:
```
doubleQuotes("hello")          // "\"hello\""
doubleQuotes(@variableName)    // "\"value\""
```

---

### guid([case])
**Description**: Generates a random globally unique identifier (GUID).

**Arguments**:
- `case` (Optional) - Use "u" or "U" for uppercase, "l" or "L" for lowercase. Default is lowercase.

**Examples**:
```
guid()           // "a3bb189e-8bf9-3888-9912-ace4e6543002" (lowercase)
guid("u")        // "A3BB189E-8BF9-3888-9912-ACE4E6543002" (uppercase)
guid("l")        // "a3bb189e-8bf9-3888-9912-ace4e6543002" (lowercase)
```

---

### htmlDecode(text)
**Description**: Decodes HTML-encoded text back to normal text. Converts HTML entities like `&lt;` back to `<`.

**Arguments**:
- `text` (Required) - The HTML-encoded text to decode

**Example**:
```
htmlDecode("&lt;div&gt;Hello&lt;/div&gt;")  // "<div>Hello</div>"
```

---

### htmlEncode(text)
**Description**: Encodes text for safe display in HTML. Converts special characters like `<`, `>`, and `&` to their HTML entity equivalents so they display correctly in browsers.

**Arguments**:
- `text` (Required) - The text to encode

**Example**:
```
htmlEncode("<div>Hello</div>")  // "&lt;div&gt;Hello&lt;/div&gt;"
```

---

### indentLine(text, indentType, count)
**Description**: Indents text by a specified number of tabs or spaces.

**Arguments**:
- `text` (Required) - The text to indent
- `indentType` (Required) - Use constant `TAB` or `SPACE`
- `count` (Required) - Number of indentations to apply

**Examples**:
```
indentLine("int x = 5;", TAB, 1)    // "\tint x = 5;"
indentLine("return x;", SPACE, 4)   // "    return x;"
indentLine("}", TAB, 2)             // "\t\t}"
```

---

### lowerCase(text)
**Description**: Converts all characters in the text to lowercase.

**Arguments**:
- `text` (Required) - The text to convert

**Example**:
```
lowerCase("HELLO WORLD")  // "hello world"
```

---

### padLeft(text, padChar, totalLength)
**Description**: Pads the text on the left side with a specified character until it reaches the desired total length.

**Arguments**:
- `text` (Required) - The text to pad
- `padChar` (Required) - The character to use for padding
- `totalLength` (Required) - The desired total length

**Example**:
```
padLeft("12", "0", 5)    // "00012"
padLeft("5", " ", 3)     // "  5"
```

---

### padRight(text, padChar, totalLength)
**Description**: Pads the text on the right side with a specified character until it reaches the desired total length.

**Arguments**:
- `text` (Required) - The text to pad
- `padChar` (Required) - The character to use for padding
- `totalLength` (Required) - The desired total length

**Example**:
```
padRight("12", "0", 5)   // "12000"
padRight("5", " ", 3)    // "5  "
```

---

### properCase(text)
**Description**: Converts text to Proper Case (Title Case) where each word starts with an uppercase letter.

**Arguments**:
- `text` (Required) - The text to convert

**Example**:
```
properCase("hello world")         // "Hello World"
properCase("the quick brown fox") // "The Quick Brown Fox"
```

---

### removePunctuation(text)
**Description**: Removes all punctuation characters from the text.

**Arguments**:
- `text` (Required) - The text to process

**Example**:
```
removePunctuation("Hello, World!")  // "Hello World"
```

---

### removeSpaces(text)
**Description**: Removes all whitespace characters (spaces, tabs, newlines) from the text.

**Arguments**:
- `text` (Required) - The text to process

**Example**:
```
removeSpaces("Hello World")      // "HelloWorld"
removeSpaces("  trim  me  ")     // "trimme"
```

---

### replace(text, findText, replaceText)
**Description**: Replaces all occurrences of a substring with another substring.

**Arguments**:
- `text` (Required) - The text to perform replacement on
- `findText` (Required) - The substring to find
- `replaceText` (Required) - The replacement substring

**Examples**:
```
replace("Hello World", " ", "_")           // "Hello_World"
replace("Dashboard Usage", " ", "")        // "DashboardUsage"
replace("literal text ya all", " ", "_")   // "literal_text_ya_all"
```

---

### upperCase(text)
**Description**: Converts all characters in the text to uppercase.

**Arguments**:
- `text` (Required) - The text to convert

**Example**:
```
upperCase("hello world")  // "HELLO WORLD"
```

---

### urlDecode(text)
**Description**: Decodes a URL-encoded string back to normal text. Converts percent-encoded characters like `%20` back to their original form.

**Arguments**:
- `text` (Required) - The URL-encoded text to decode

**Example**:
```
urlDecode("hello%20world")                           // "hello world"
urlDecode("user%40email.com")                        // "user@email.com"
```

---

### urlEncode(text)
**Description**: Encodes text for safe use in URLs. Converts special characters to percent-encoded format. URLs cannot contain spaces and must use ASCII characters only.

**Arguments**:
- `text` (Required) - The text to encode

**Examples**:
```
urlEncode("hello world")                             // "hello%20world"
urlEncode("rob+thomas@gmail.com")                    // "rob%2bthomas%40gmail.com"
```

---

## Best Practices

1. **Use Descriptive Variable Names**: `@userName` is better than `@u`
2. **Group Related Declarations**: Keep inputs together, expressions together
3. **Comment Complex Logic**: Use comments to explain conditional flows
4. **Test Incrementally**: Build complex expressions step by step
5. **Format for Readability**: Use proper indentation and line breaks
6. **Leverage Constants**: Use `TAB`, `SPACE`, `NEWLINE` instead of escape sequences

---

## Error Handling

Common errors and solutions:

### Syntax Errors
- **Missing semicolon**: Every declaration must end with `;`
- **Unmatched quotes**: Strings must use `"` and escape with `""`
- **Invalid variable name**: Variables must start with `@`

### Runtime Errors
- **Undefined variable**: Ensure variables are declared before use
- **Function argument mismatch**: Check function signatures
- **Type mismatches**: Most values are treated as strings

---

## Integration Example

Complete integration example:

```csharp
using CygSoft.Qik;
using CygSoft.Qik.Functions;
using Qik.UiWidgets;

public class TemplateGenerator
{
    private readonly IInterpreter _interpreter;
    private readonly IFunctionFactory _functionFactory;
    
    public TemplateGenerator(IPluginLoader pluginLoader)
    {
        _interpreter = new Interpreter();
        _functionFactory = new FunctionFactory(pluginLoader);
    }
    
    public string Generate(string script)
    {
        var terminal = _interpreter.Interpret(_functionFactory, script);
        
        // Get all generated values
        var results = new Dictionary<string, string>();
        foreach (var symbol in terminal.Symbols)
        {
            results[symbol] = terminal.GetValue(symbol);
        }
        
        return results["@output"]; // Return main output
    }
    
    public UiWidget[] GetInputFields(string script)
    {
        var widgetFactory = new UiWidgetFactory();
        return widgetFactory.BuildFromScript(script);
    }
}
```

---

## Additional Resources

- **Source Code**: https://github.com/rob-bl8ke/Qik
- **NuGet Package**: https://www.nuget.org/packages/rob_bl8ke.Qik/
- **ANTLR Documentation**: https://www.antlr.org/

---

## License

GPL-3.0-or-later

---

*This guide is based on Qik version 1.0.1. Features and syntax may evolve in future versions.*
