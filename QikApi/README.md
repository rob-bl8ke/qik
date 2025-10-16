# Qik REST API

A comprehensive REST API that exposes the functionality of the [Qik template generation library](https://github.com/rob-bl8ke/Qik). This API provides endpoints for script interpretation, UI widget extraction, expression evaluation, and function discovery.

## Features

- **Script Interpretation**: Execute complete Qik scripts and retrieve all variable values
- **UI Widget Extraction**: Extract UI metadata from scripts for dynamic form generation
- **Expression Evaluation**: Evaluate individual expressions with context variables
- **Function Discovery**: Get documentation for all available Qik functions
- **Interactive Swagger UI**: Full API documentation and testing interface

## Quick Start

### Running the API

```bash
cd QikApi
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`).

### Accessing Swagger UI

Navigate to the root URL in your browser to access the interactive Swagger UI:

```
https://localhost:5001
```

## API Endpoints

### 1. Interpret Script
**POST** `/api/qik/interpret`

Interprets a complete Qik script and returns all variable values.

**Request Body:**
```json
{
  "script": "@name => \"John\"; @greeting => \"Hello, \" + @name;",
  "variables": {
    "@name": "Alice"
  }
}
```

**Response:**
```json
{
  "values": {
    "@name": "Alice",
    "@greeting": "Hello, Alice"
  },
  "symbols": ["@name", "@greeting"],
  "inputSymbols": [],
  "success": true,
  "errorMessage": null
}
```

### 2. Extract UI Widgets
**POST** `/api/qik/widgets`

Extracts UI widget metadata from a Qik script for dynamic form generation.

**Request Body:**
```json
{
  "script": "[title = \"User Name\", type = \"text\"] @userName => \"Guest\";"
}
```

**Response:**
```json
{
  "widgets": [
    {
      "variableName": "@userName",
      "title": "User Name",
      "type": "text",
      "defaultValue": "Guest"
    }
  ],
  "success": true,
  "errorMessage": null
}
```

### 3. Evaluate Expression
**POST** `/api/qik/evaluate`

Evaluates a single expression with optional context variables.

**Request Body:**
```json
{
  "expression": "upperCase(@name) + \" - \" + currentDate(\"yyyy-MM-dd\")",
  "context": {
    "@name": "john doe"
  }
}
```

**Response:**
```json
{
  "result": "JOHN DOE - 2025-10-16",
  "success": true,
  "errorMessage": null
}
```

### 4. Get Available Functions
**GET** `/api/qik/functions`

Returns documentation for all available Qik functions.

**Response:**
```json
[
  {
    "name": "camelCase",
    "description": "Converts text to camelCase format",
    "signature": "camelCase(text)",
    "example": "camelCase(\"Hello World\") // \"helloWorld\"",
    "category": "Text Transformation"
  },
  ...
]
```

### 5. Health Check
**GET** `/api/qik/health`

Returns the health status of the API.

**Response:**
```json
{
  "status": "healthy",
  "service": "Qik API",
  "version": "1.0.0"
}
```

## Example Use Cases

### 1. Code Generation
```bash
curl -X POST https://localhost:5001/api/qik/interpret \
  -H "Content-Type: application/json" \
  -d '{
    "script": "@className => \"UserService\"; @namespace => \"MyApp.Services\"; @classFile => \"namespace \" + @namespace + NEWLINE + \"{\" + NEWLINE + indentLine(\"public class \" + @className, TAB, 1) + NEWLINE + indentLine(\"{\", TAB, 1) + NEWLINE + indentLine(\"}\", TAB, 1) + NEWLINE + \"}\";"
  }'
```

### 2. Dynamic Form Generation
```bash
curl -X POST https://localhost:5001/api/qik/widgets \
  -H "Content-Type: application/json" \
  -d '{
    "script": "[title = \"Email\", type = \"email\"] @email => \"user@example.com\"; [title = \"Age\", type = \"number\"] @age => \"30\";"
  }'
```

### 3. Text Transformation
```bash
curl -X POST https://localhost:5001/api/qik/evaluate \
  -H "Content-Type: application/json" \
  -d '{
    "expression": "camelCase(replace(@input, \" \", \"_\"))",
    "context": {
      "@input": "My Variable Name"
    }
  }'
```

### 4. URL Builder
```bash
curl -X POST https://localhost:5001/api/qik/interpret \
  -H "Content-Type: application/json" \
  -d '{
    "script": "@baseUrl => \"https://api.example.com\"; @endpoint => \"users\"; @query => \"john doe\"; @fullUrl => @baseUrl + \"/\" + @endpoint + \"?q=\" + urlEncode(@query);"
  }'
```

## Qik Script Syntax

### Variables
All variables start with `@` and use `=>` for assignment:
```
@name => "Alice";
@age => "30";
```

### String Concatenation
Use `+` to concatenate strings:
```
@fullName => @firstName + " " + @lastName;
```

### Functions
Qik provides many built-in functions:
```
@camel => camelCase(@input);
@encoded => base64Encode(@data);
@formatted => upperCase(replace(@text, " ", "_"));
```

### Conditionals

**Ternary (IIF):**
```
@result => @count == "0" ? "empty" : "not empty";
```

**If-Else:**
```
@color => 
    if @score == "A" then "green"
    else if @score == "B" then "yellow"
    else "red";
```

**Switch:**
```
@dayType =>
    switch @day
        case "Saturday" then "Weekend"
        case "Sunday" then "Weekend"
        else "Weekday";
```

### Constants
- `TAB` - Tab character
- `SPACE` - Space character
- `NEWLINE` - Newline character

## Available Functions

### Text Transformation
- `camelCase(text)` - Convert to camelCase
- `upperCase(text)` - Convert to UPPERCASE
- `lowerCase(text)` - Convert to lowercase
- `properCase(text)` - Convert to Proper Case
- `abbreviate(text)` - Extract first letters
- `replace(text, find, replace)` - Replace substrings
- `removePunctuation(text)` - Remove punctuation
- `removeSpaces(text)` - Remove whitespace

### Padding & Formatting
- `padLeft(text, char, length)` - Pad left side
- `padRight(text, char, length)` - Pad right side
- `indentLine(text, type, count)` - Indent with tabs/spaces
- `doubleQuotes(text)` - Wrap in quotes

### Encoding
- `base64Encode(text)` - Encode to Base64
- `base64Decode(text)` - Decode from Base64
- `urlEncode(text)` - Encode for URLs
- `urlDecode(text)` - Decode from URL format
- `htmlEncode(text)` - Encode for HTML
- `htmlDecode(text)` - Decode from HTML

### Generation
- `guid([case])` - Generate GUID
- `currentDate([format])` - Get current date

## Development

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Project Structure
```
QikApi/
├── Controllers/
│   └── QikController.cs       # API endpoints
├── Models/
│   ├── InterpretRequest.cs    # Request DTOs
│   ├── InterpretResponse.cs   # Response DTOs
│   └── ...
├── Services/
│   ├── IQikService.cs         # Service interface
│   └── QikService.cs          # Service implementation
└── Program.cs                 # Application configuration
```

## Error Handling

All endpoints return appropriate HTTP status codes:
- `200 OK` - Successful operation
- `400 Bad Request` - Invalid input or script errors

Error responses include details:
```json
{
  "success": false,
  "errorMessage": "Interpretation error: Syntax error at line 1"
}
```

## CORS Configuration

The API is configured with permissive CORS in development mode to allow testing from any origin.

## License

This API is part of the Qik project, licensed under GPL-3.0-or-later.

## Resources

- **Qik Library**: https://github.com/rob-bl8ke/Qik
- **NuGet Package**: https://www.nuget.org/packages/rob_bl8ke.Qik/
- **Technical Guide**: See `docs/technical-guide.md` in the main repository
