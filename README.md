
# Qik

A script-based template generation library and REST API built on ANTLR4. Qik provides a custom scripting language for defining variables, expressions, conditional logic, and text transformation functions — useful for dynamic code generation, document templating, and building configurable UI forms.

**NuGet**: [rob_bl8ke.Qik](https://www.nuget.org/packages/rob_bl8ke.Qik/)  
**License**: GPL-3.0-or-later

---

## Solution Structure

| Project | Target | Description |
|---------|--------|-------------|
| **Qik** | net7.0 | Core library — interpreter, symbol table, functions, ANTLR visitors |
| **QikAntlr** | net7.0 | ANTLR4 grammar (`QikTemplate.g4`) and generated parser/lexer |
| **QikApi** | net9.0 | ASP.NET Core Web API exposing the library over HTTP |
| **QikTests** | net7.0 | Unit tests for the core library (NUnit) |
| **QikApiTests** | net9.0 | Unit + integration tests for the API (NUnit) |

---

## Quick Start

### Prerequisites

- .NET 7 SDK (core library & tests)
- .NET 9 SDK (API project)

### Build & Run

```bash
dotnet restore
dotnet build
```

Run the API:

```bash
dotnet run --project QikApi
```

The API launches with Swagger UI at the root URL for interactive exploration.

### Run Tests

```bash
dotnet test
```

Or target a specific project:

```bash
dotnet test ./QikTests/QikTests.csproj
dotnet test ./QikApiTests/QikApiTests.csproj
```

---

## Qik Scripting Language

### Variables

All variables start with `@` and are declared with the `=>` operator:

```
@name => "Alice";
@greeting => "Hello, " + @name + "!";
```

### UI Widget Metadata

Attach metadata for dynamic form generation:

```
[title = "Enter Name", type = "text"] @userName => "Default";
```

### String Concatenation

```
@fullName => @firstName + " " + @lastName;
```

### Constants

`TAB`, `SPACE`, `NEWLINE` — built-in constants for whitespace characters.

### Conditional Logic

**Ternary:**
```
@status => @count == "0" ? "empty" : "has items";
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

### Comments

```
/* Block comment */
// Line comment
@var => "value"; // Inline comment
```

---

## Built-in Functions

### Text Transformation
`camelCase`, `upperCase`, `lowerCase`, `properCase`, `replace`, `abbreviate`, `removePunctuation`, `removeSpaces`

### Padding & Formatting
`padLeft`, `padRight`, `indentLine`, `doubleQuote`

### Encoding
`base64Encode`, `base64Decode`, `urlEncode`, `urlDecode`, `htmlEncode`, `htmlDecode`

### Generation
`guid`, `currentDate`

Functions compose naturally:

```
@result => upperCase(camelCase(@input));
@encoded => base64Encode(urlEncode(@data));
```

### Plugin System

Custom functions can be loaded from DLLs placed in a `Plugins/` folder alongside the application. Plugins extend `BaseFunction` and are annotated with `[QikFunction]`.

---

## REST API Endpoints

Base path: `/api/qik`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/interpret` | Interpret a full script, return all variable values |
| POST | `/generate` | Generate documents from script + fragment templates |
| POST | `/evaluate` | Evaluate a single expression with context variables |
| POST | `/widgets` | Extract UI widget metadata from a script |
| GET | `/functions` | List all available functions with descriptions |
| GET | `/health` | Health check |

### Content Encoding

The `/interpret`, `/generate`, and `/widgets` endpoints accept an optional `?contentEncoding=base64` query parameter for binary-safe transport of scripts and templates.

### Example: Interpret

```http
POST /api/qik/interpret
Content-Type: application/json

{
  "script": "@class => \"UserService\"; @code => \"public class \" + @class + \" { }\";"
}
```

Response:

```json
{
  "success": true,
  "values": {
    "@class": "UserService",
    "@code": "public class UserService { }"
  }
}
```

### Example: Generate Documents

```http
POST /api/qik/generate
Content-Type: application/json

{
  "script": "@className => \"User\"; @namespace => \"MyApp.Models\";",
  "fragments": {
    "classTemplate": "namespace @{namespace};\n\npublic class @{className}\n{\n}\n"
  },
  "documents": {
    "models/User.cs": "{classTemplate}"
  },
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

### CORS

Configured by default for `http://localhost:4200` (Angular frontend).

---

## Architecture

```
┌──────────────┐      ┌──────────────┐
│   QikApi     │─────▶│     Qik      │
│ (ASP.NET 9)  │      │  (Library)   │
└──────────────┘      └──────┬───────┘
                             │
                      ┌──────▼───────┐
                      │   QikAntlr   │
                      │  (Grammar)   │
                      └──────────────┘
```

1. **QikAntlr** — defines the `QikTemplate.g4` grammar and generates the lexer/parser via ANTLR4.
2. **Qik** — walks the parse tree with visitors (`UserInputVisitor`, `ExpressionVisitor`, `UiWidgetVisitor`) to populate a `SymbolTable`. Functions are resolved through a `FunctionFactory` with a plugin extension point.
3. **QikApi** — thin HTTP layer using `QikService` to bridge requests to the interpreter.

---

## Development Notes

### ANTLR4 Grammar (VS Code)

Install the [ANTLR4 grammar syntax support](https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4) extension with these workspace settings:

```json
{
  "antlr4.generation": {
    "mode": "external",
    "language": "CSharp",
    "visitors": true,
    "outputDir": "_antlr",
    "package": "CygSoft.Qik.Antlr"
  }
}
```

Changes to `QikAntlr/QikTemplate.g4` auto-generate files into `QikAntlr/_antlr/`.

### Docker (SonarQube)

A `docker-compose.yml` provides a local SonarQube instance for code analysis:

```bash
docker compose up -d
```

### Publishing

```bash
dotnet publish -c Release
```

### SDK Management

Use `dotnet sdk check` to verify installed SDK versions. A `global.json` can pin the SDK:

```json
{
  "sdk": {
    "version": "7.0.203",
    "rollForward": "latestFeature"
  }
}
```

---

## Documentation

| Document | Description |
|----------|-------------|
| [Technical Guide](docs/technical-guide.md) | Full language reference — syntax, all functions, conditional logic, advanced features, and interpreter usage |
| [API Overview](docs/OVERVIEW.md) | Detailed REST API documentation with endpoint descriptions and usage examples |
| [Getting Started](docs/GETTING-STARTED.md) | Content encoding guide — how to use Base64 vs plain text with the API |
| [Generate Endpoint Summary](docs/GENERATE-ENDPOINT-SUMMARY.md) | Implementation details for the `/api/qik/generate` endpoint |
| [Configurable Placeholders](docs/demo-configurable-placeholders.md) | How to use custom placeholder prefixes/suffixes in template generation |
| [Project Summary](docs/PROJECT-SUMMARY.md) | High-level overview of what was built and the feature checklist |
| [VS Code Syntax Highlighting](docs/vs-code-syntax-highlighting.md) | Guide to creating a VS Code extension for Qik language syntax highlighting |
| [SonarQube Setup](docs/sonarqube/sonarqube-local-setup-guide.md) | Running SonarQube locally with Docker for code analysis |
| [HTTPS Certificate](docs/cert/certificate-import.md) | Configuring persistent HTTPS dev certificates in .NET dev containers |
| [Postman Collection](docs/http/QikApi.postman_collection.json) | Importable Postman collection for testing all API endpoints |
