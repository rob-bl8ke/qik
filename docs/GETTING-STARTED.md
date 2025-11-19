## Encoding Support & contentEncoding Parameter

Many endpoints support both plain text and Base64-encoded input/output. Use the optional `contentEncoding` query parameter to specify encoding:

- **Plain Text (default):**
  - Send/receive all fields as UTF-8 strings.
  - No `contentEncoding` parameter needed.
- **Base64:**
  - Set `?contentEncoding=base64` in the query string.
  - All string fields in the request and response will be Base64-encoded.
  - Useful for binary-safe transport, non-UTF8 content, or when embedding templates/code.

**Endpoints supporting `contentEncoding`:**
- `/api/qik/generate`
- `/api/qik/interpret`
- `/api/qik/widgets`

**Endpoints NOT supporting `contentEncoding`:**
- `/api/qik/evaluate` (always plain text)
- `/api/qik/functions`, `/api/qik/health`

If `contentEncoding` is omitted or set to anything other than `base64`, plain text is used.

# Qik API - Getting Started Guide

## Overview

The Qik REST API is a comprehensive web service that exposes all functionality of the Qik template generation library through simple HTTP endpoints. This guide will help you get started quickly.

## What is Qik?

Qik is a script-based template generation library that provides:
- **Dynamic Variables** - Define and manipulate text values
- **Text Transformations** - Convert text using 20+ built-in functions
- **Conditional Logic** - Use if-else, switch, and ternary operators
- **Encoding/Decoding** - Handle Base64, URL, and HTML encoding
- **Code Generation** - Generate formatted code with proper indentation
- **Form Generation** - Extract UI metadata for dynamic form creation

## Quick Start

### 1. Start the API

```powershell
cd QikApi
dotnet run
```

The API will start at `http://localhost:5213` (or the configured port).

### 2. Open Swagger UI

Navigate to `http://localhost:5213` in your browser to access the interactive API documentation.

### 3. Try Your First Request

**Simple Text Transformation:**

```http
POST http://localhost:5213/api/qik/evaluate
Content-Type: application/json

{
  "expression": "upperCase(\"hello world\")"
}
```

**Response:**
```json
{
  "result": "HELLO WORLD",
  "success": true
}
```

## Core Concepts

### Variables

Variables in Qik scripts always start with `@`:

```qik
@name => "John Doe";
@email => "john@example.com";
```

### Functions

Qik provides 20+ built-in functions for text manipulation:

```qik
@camel => camelCase("Hello World");        // "helloWorld"
@upper => upperCase("hello");              // "HELLO"
@encoded => base64Encode("data");          // "ZGF0YQ=="
```

### String Concatenation

Use `+` to combine strings:

```qik
@firstName => "John";
@lastName => "Doe";
@fullName => @firstName + " " + @lastName;
```

### Conditionals

**Ternary Operator:**
```qik
@status => @count == "0" ? "empty" : "has items";
```

**If-Else:**
```qik
@grade =>
    if @score == "90" then "A"
    else if @score == "80" then "B"
    else "C";
```

**Switch:**
```qik
@type =>
    switch @day
        case "Saturday" then "Weekend"
        case "Sunday" then "Weekend"
        else "Weekday";
```

### Configurable Placeholders

The generate endpoint supports configurable placeholder formats for maximum template flexibility:

**Default format (@{variable}):**
```json
{
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

**Other common formats:**
- **Dollar syntax**: `"${", "}"` → `${variable}`
- **Mustache style**: `"{{", "}}"` → `{{variable}}`
- **Bracket style**: `"[[", "]]"` → `[[variable]]`

This allows integration with existing template systems while leveraging Qik's powerful scripting capabilities.

## API Endpoints

### 1. POST /api/qik/interpret

Execute a complete Qik script and get all variable values.

**Use Case:** Full script execution, code generation, complex transformations

**Plain Text (default):**
```json
{
  "script": "@name => \"Alice\"; @greeting => \"Hello, \" + @name + \"!\";"
}
```

**Base64:**
```http
POST /api/qik/interpret?contentEncoding=base64
{
  "script": "QG5hbWUgPT4gXCJBbGljZVwiOyBAbmFtZSA9PiBcIkhlbGxvLCBcIiArIEBuYW1lICsgXCIhXCI7"
}
```

### 2. POST /api/qik/generate

Generate multiple documents using fragments and configurable placeholder formats.

**Use Case:** Multi-file generation, template composition, document creation

**Plain Text (default):**
```json
{
  "script": "@className => \"User\"; @namespace => \"MyApp.Models\";",
  "fragments": {
    "classTemplate": "namespace @{namespace};\n\npublic class @{className}\n{\n}\n"
  },
  "documents": {
    "models/User.cs": "{classTemplate}"
  },
  "inputs": {
    "@className": "Customer"
  },
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

**Base64:**
```http
POST /api/qik/generate?contentEncoding=base64
{
  "script": "QGNsYXNzTmFtZSA9PiAiVXNlciI7IEBuYW1lc3BhY2UgPT4gIk15QXBwLk1vZGVscyI7",
  "fragments": {
    "classTemplate": "bmFtZXNwYWNlIEB7bmFtZXNwYWNlfTtcblxucHVibGljIGNsYXNzIEB7Y2xhc3NOYW1lfVxue1xufQ=="
  },
  "documents": {
    "models/User.cs": "{classTemplate}"
  },
  "inputs": {
    "@className": "Customer"
  },
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

### 3. POST /api/qik/evaluate

Evaluate a single expression with optional context.

**Use Case:** Quick transformations, testing functions, single calculations

**Example:**
```json
{
  "expression": "camelCase(@input)",
  "context": {
    "@input": "Hello World"
  }
}
```

### 4. POST /api/qik/widgets

Extract UI widget metadata from a script.

**Use Case:** Dynamic form generation, input field discovery

**Plain Text (default):**
```json
{
  "script": "[title = \"Name\", type = \"text\"] @name => \"Guest\";"
}
```

**Base64:**
```http
POST /api/qik/widgets?contentEncoding=base64
{
  "script": "W3RpdGxlID0gXCJOYW1lXCIsIHR5cGUgPSBcInRleHRcIl0gQG5hbWUgPT4gXCJHdWVzdFwiOw=="
}
```

### 5. GET /api/qik/functions

Get documentation for all available functions.

**Use Case:** Function discovery, documentation reference

## Common Use Cases

### 1. Document Generation

Generate multiple files using fragments and templates:

```http
POST /api/qik/generate
{
  "script": "QGNsYXNzTmFtZSA9PiAiVXNlciI7IEBuYW1lc3BhY2UgPT4gIk15QXBwLk1vZGVscyI7",
  "fragments": {
    "classTemplate": "bmFtZXNwYWNlIEB7bmFtZXNwYWNlfTtcblxucHVibGljIGNsYXNzIEB7Y2xhc3NOYW1lfVxue1xufQ==",
    "interfaceTemplate": "bmFtZXNwYWNlIEB7bmFtZXNwYWNlfTtcblxucHVibGljIGludGVyZmFjZSBJQHtjbGFzc05hbWV9XG57XG59"
  },
  "documents": {
    "models/User.cs": "{classTemplate}",
    "interfaces/IUser.cs": "{interfaceTemplate}"
  },
  "inputs": {
    "@className": "Customer",
    "@namespace": "MyApp.Domain"
  },
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

Result: Multiple Base64 encoded documents ready for file creation.

### 2. Text Transformation API

Transform user input on-the-fly:

```http
POST /api/qik/evaluate
{
  "expression": "camelCase(removeSpaces(@input))",
  "context": {
    "@input": "My Variable Name"
  }
}
```

Result: `"myVariableName"`

### 3. Code Generator

Generate formatted code structures:

```http
POST /api/qik/interpret
{
  "script": "@class => \"UserService\"; @code => \"public class \" + @class + NEWLINE + \"{\" + NEWLINE + indentLine(\"// Implementation\", TAB, 1) + NEWLINE + \"}\";"
}
```

### 4. Dynamic Form Builder

Extract form metadata and generate UI:

```http
POST /api/qik/widgets
{
  "script": "[title=\"Email\", type=\"email\"] @email => \"\"; [title=\"Age\", type=\"number\"] @age => \"18\";"
}
```

Then use the response to build a dynamic form in your frontend.

### 5. URL Builder

Create safe URLs with encoding:

```http
POST /api/qik/interpret
{
  "script": "@base => \"https://api.example.com\"; @query => \"user search\"; @url => @base + \"/search?q=\" + urlEncode(@query);"
}
```

### 6. Data Encoder/Decoder

Handle encoding transformations:

```http
POST /api/qik/evaluate
{
  "expression": "base64Encode(urlEncode(@data))",
  "context": {
    "@data": "secret+data@2025"
  }
}
```

## Function Reference

### Text Transformation
- `camelCase(text)` - helloWorld
- `upperCase(text)` - HELLO WORLD
- `lowerCase(text)` - hello world
- `properCase(text)` - Hello World
- `abbreviate(text)` - Extract first letters
- `replace(text, find, replace)` - Find and replace
- `removePunctuation(text)` - Remove special chars
- `removeSpaces(text)` - Remove whitespace

### Formatting
- `padLeft(text, char, length)` - Pad left
- `padRight(text, char, length)` - Pad right
- `indentLine(text, type, count)` - Add indentation
- `doubleQuotes(text)` - Wrap in quotes

### Encoding
- `base64Encode(text)` / `base64Decode(text)`
- `urlEncode(text)` / `urlDecode(text)`
- `htmlEncode(text)` / `htmlDecode(text)`

### Generation
- `guid([case])` - Generate GUID
- `currentDate([format])` - Get current date

### Constants
- `TAB` - Tab character
- `SPACE` - Space character
- `NEWLINE` - Newline character

## Integration Examples


### JavaScript/Fetch

**Simple Transformation (plain text):**
```javascript
async function transformText(input) {
  const response = await fetch('http://localhost:5213/api/qik/evaluate', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      expression: 'camelCase(@input)',
      context: { '@input': input }
    })
  });
  const data = await response.json();
  return data.result;
}
```

**Document Generation (Base64):**
```javascript
async function generateDocuments(className, namespace) {
  const response = await fetch('http://localhost:5213/api/qik/generate?contentEncoding=base64', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      script: btoa(`@className => "${className}"; @namespace => "${namespace}";`),
      fragments: {
        classTemplate: btoa(`namespace @{namespace};\n\npublic class @{className}\n{\n}`)
      },
      documents: {
        [`models/${className}.cs`]: "{classTemplate}"
      },
      placeholderPrefix: "@{",
      placeholderSuffix: "}"
    })
  });
  const data = await response.json();
  // Decode Base64 content
  const decodedDocuments = {};
  for (const [path, content] of Object.entries(data.documents)) {
    decodedDocuments[path] = atob(content);
  }
  return decodedDocuments;
}
```


### C# / HttpClient

**Simple Evaluation (plain text):**
```csharp
using System.Net.Http.Json;

var client = new HttpClient { BaseAddress = new Uri("http://localhost:5213") };

var request = new {
  expression = "upperCase(@name)",
  context = new Dictionary<string, string> { ["@name"] = "john" }
};

var response = await client.PostAsJsonAsync("/api/qik/evaluate", request);
var result = await response.Content.ReadFromJsonAsync<EvaluateResponse>();

Console.WriteLine(result.Result); // "JOHN"
```

**Document Generation (Base64):**
```csharp
var generateRequest = new {
  script = Convert.ToBase64String(Encoding.UTF8.GetBytes("@className => \"User\"; @namespace => \"MyApp.Models\";")),
  fragments = new Dictionary<string, string> {
    ["classTemplate"] = Convert.ToBase64String(Encoding.UTF8.GetBytes("namespace @{namespace};\n\npublic class @{className}\n{\n}"))
  },
  documents = new Dictionary<string, string> {
    ["models/User.cs"] = "{classTemplate}"
  },
  placeholderPrefix = "@{",
  placeholderSuffix = "}"
};

var generateResponse = await client.PostAsJsonAsync("/api/qik/generate?contentEncoding=base64", generateRequest);
var generateResult = await generateResponse.Content.ReadFromJsonAsync<GenerateResponse>();

// Decode Base64 content
foreach (var doc in generateResult.Documents)
{
  var content = Encoding.UTF8.GetString(Convert.FromBase64String(doc.Value));
  Console.WriteLine($"{doc.Key}:\n{content}");
}
```


### Python / Requests

**Simple Evaluation (plain text):**
```python
import requests

response = requests.post('http://localhost:5213/api/qik/evaluate', json={
  'expression': 'camelCase(@input)',
  'context': {'@input': 'hello world'}
})

result = response.json()
print(result['result'])  # "helloWorld"
```

**Document Generation (Base64):**
```python
import requests
import base64

# Prepare Base64 encoded content
script = base64.b64encode(b'@className => "User"; @namespace => "MyApp.Models";').decode()
template = base64.b64encode(b'namespace @{namespace};\n\npublic class @{className}\n{\n}').decode()

response = requests.post('http://localhost:5213/api/qik/generate?contentEncoding=base64', json={
  'script': script,
  'fragments': {
    'classTemplate': template
  },
  'documents': {
    'models/User.cs': '{classTemplate}'
  },
  'inputs': {
    '@className': 'Customer'
  },
  'placeholderPrefix': '@{',
  'placeholderSuffix': '}'
})

result = response.json()

# Decode Base64 content
for path, content in result['documents'].items():
  decoded_content = base64.b64decode(content).decode()
  print(f"{path}:\n{decoded_content}")
```

### cURL

**Simple Evaluation:**
```bash
curl -X POST http://localhost:5213/api/qik/evaluate \
  -H "Content-Type: application/json" \
  -d '{
    "expression": "upperCase(@text)",
    "context": {"@text": "hello"}
  }'
```


**Document Generation (Base64):**
```bash
curl -X POST "http://localhost:5213/api/qik/generate?contentEncoding=base64" \
  -H "Content-Type: application/json" \
  -d '{
    "script": "QGNsYXNzTmFtZSA9PiAiVXNlciI7IEBuYW1lc3BhY2UgPT4gIk15QXBwLk1vZGVscyI7",
    "fragments": {
      "classTemplate": "bmFtZXNwYWNlIEB7bmFtZXNwYWNlfTtcblxucHVibGljIGNsYXNzIEB7Y2xhc3NOYW1lfVxue1xufQ=="
    },
    "documents": {
      "models/User.cs": "{classTemplate}"
    },
    "placeholderPrefix": "@{",
    "placeholderSuffix": "}"
  }'
```

## Error Handling

All endpoints return structured error responses:

```json
{
  "success": false,
  "errorMessage": "Interpretation error: Syntax error at line 1"
}
```

**HTTP Status Codes:**
- `200 OK` - Successful operation
- `400 Bad Request` - Invalid input or script error

## Best Practices

1. **Use `/evaluate` for Simple Operations** - When you just need to transform a single value
2. **Use `/interpret` for Complex Scripts** - When you need multiple variables and logic
3. **Cache Function List** - The `/functions` endpoint data rarely changes
4. **Extract Widgets Once** - Parse UI widgets on page load, not on every render
5. **Handle Errors Gracefully** - Always check the `success` field in responses

## Performance Tips

- The API creates a new interpreter instance per request (stateless)
- For high-volume scenarios, consider caching interpreted scripts
- Use `/evaluate` instead of `/interpret` when possible (simpler, faster)
- Batch multiple transformations in a single script when appropriate

## Testing

Use the included `example-requests.http` file with REST Client extension in VS Code, or use the Swagger UI for interactive testing.

## Deployment

The API is a standard ASP.NET Core application and can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
- IIS
- Self-hosted on Windows/Linux

## Next Steps

1. **Explore the Swagger UI** - Try all endpoints interactively
2. **Review `example-requests.http`** - See more request examples
3. **Read the Technical Guide** - Deep dive into Qik syntax (`docs/technical-guide.md`)
4. **Integrate with Your App** - Use the client examples above
5. **Extend with Plugins** - Qik supports custom function plugins

## Support

- **Repository**: https://github.com/rob-bl8ke/Qik
- **NuGet**: https://www.nuget.org/packages/rob_bl8ke.Qik/
- **Issues**: https://github.com/rob-bl8ke/Qik/issues

## License

GPL-3.0-or-later
