# QikApi - Complete REST API for Qik Template Generation

## 🎯 Overview

**QikApi** is a comprehensive .NET 9.0 Web API that exposes the full functionality of the Qik template generation library through simple REST endpoints. Built following the technical guide, it provides a clean HTTP interface for script interpretation, text transformation, and dynamic form generation.

## ✨ What Can You Do With This API?

### 1. Transform Text
```http
POST /api/qik/evaluate
{
  "expression": "upperCase(camelCase(@input))",
  "context": { "@input": "hello world" }
}
→ "HELLOWORLD"
```


### 2. Generate Documents (Plain Text & Base64)
#### Plain Text (default)
```http
POST /api/qik/generate
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
→ { "documents": { "models/User.cs": "namespace MyApp.Models;\n\npublic class User\n{\n}\n" } }
```

#### Base64 (set `contentEncoding`)
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
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
→ { "documents": { "models/User.cs": "bmFtZXNwYWNlIE15QXBwLk1vZGVscztcblxucHVibGljIGNsYXNzIFVzZXJcbntcbn0=" } }
```


### 3. Generate Code (Plain Text & Base64)
#### Plain Text (default)
```http
POST /api/qik/interpret
{
  "script": "@class => \"UserService\"; @code => \"public class \" + @class + \" { }\""
}
→ { "@class": "UserService", "@code": "public class UserService { }" }
```

#### Base64 (set `contentEncoding`)
```http
POST /api/qik/interpret?contentEncoding=base64
{
  "script": "QGNsYXNzID0+IFwiVXNlclNlcnZpY2VcIjsgQGNvZGUgPT4gXCJwdWJsaWMgY2xhc3MgXCIgKyBAY2xhc3MgKyBcIiB7IH1cIg=="
}
→ { "@class": "VXNlclNlcnZpY2U=", "@code": "cHVibGljIGNsYXNzIFVzZXJzZXJ2aWNlIHsgfQ==" }
```


### 4. Build Forms Dynamically (Plain Text & Base64)
#### Plain Text (default)
```http
POST /api/qik/widgets
{
  "script": "[title=\"Name\", type=\"text\"] @name => \"Guest\";"
}
→ [{ "variableName": "@name", "title": "Name", "type": "text", "defaultValue": "Guest" }]
```

#### Base64 (set `contentEncoding`)
```http
POST /api/qik/widgets?contentEncoding=base64
{
  "script": "W3RpdGxlPVwiTmFtZVwiLCB0eXBlPVwidGV4dFwiXSBAbmFtZSA9PiBcIkd1ZXN0XCI7"
}
→ W3sidmFyaWFibGVOYW1lIjoiQG5hbWUiLCJ0aXRsZSI6Ik5hbWUiLCJ0eXBlIjoidGV4dCIsImRlZmF1bHRWYWx1ZSI6Ikd1ZXN0In1d
```

### 5. Encode/Decode Data
```http
POST /api/qik/evaluate
{
  "expression": "base64Encode(urlEncode(@data))",
  "context": { "@data": "hello world" }
}
→ "aGVsbG8lMjB3b3JsZA=="
```

### 6. Generate Dynamic Content
```http
POST /api/qik/evaluate
{
  "expression": "guid(\"u\") + \"_\" + currentDate(\"yyyyMMdd\")"
}
→ "A3BB189E-8BF9-3888-9912-ACE4E6543002_20251016"
```

## 🚀 Quick Start

### Start the API
```powershell
cd QikApi
dotnet run
```

### Open Swagger UI
Navigate to: **http://localhost:5213**

### Try Your First Request
```bash
curl -X POST http://localhost:5213/api/qik/evaluate \
  -H "Content-Type: application/json" \
  -d '{"expression": "upperCase(\"hello world\")"}'
```

## 📚 API Endpoints

| Method | Endpoint | Purpose | Encoding |
|--------|----------|---------|----------|
| POST | `/api/qik/interpret` | Execute complete Qik scripts | Plain text (default), Base64 (optional) |
| POST | `/api/qik/generate` | Generate documents from fragments with configurable placeholders | Plain text (default), Base64 (optional) |
| POST | `/api/qik/evaluate` | Evaluate single expressions | Plain text only |
| POST | `/api/qik/widgets` | Extract UI widget metadata | Plain text (default), Base64 (optional) |
| GET | `/api/qik/functions` | List all available functions | - |
| GET | `/api/qik/health` | Health check | - |
## 🗝️ Encoding Support & contentEncoding Parameter

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

**Example:**

```http
POST /api/qik/generate?contentEncoding=base64
{
  "script": "QGNsYXNzTmFtZSA9PiAiVXNlciI7IEBuYW1lc3BhY2UgPT4gIk15QXBwLk1vZGVscyI7",
  ...
}
→ { "documents": { ... } }
```

If `contentEncoding` is omitted or set to anything other than `base64`, plain text is used.


## 🔧 Available Functions (20+)

### Text Transformation
`camelCase` • `upperCase` • `lowerCase` • `properCase` • `abbreviate` • `replace` • `removePunctuation` • `removeSpaces`

### Formatting
`padLeft` • `padRight` • `indentLine` • `doubleQuotes`

### Encoding
`base64Encode` • `base64Decode` • `urlEncode` • `urlDecode` • `htmlEncode` • `htmlDecode`

### Generation
`guid` • `currentDate`

## 📖 Documentation Files

| File | Description |
|------|-------------|
| `README.md` | Complete API reference with examples |
| `GETTING-STARTED.md` | Quick start guide with integration examples |
| `PROJECT-SUMMARY.md` | Technical overview and architecture |
| `example-requests.http` | 15+ test requests for REST Client |
| `QikApi.postman_collection.json` | Postman collection |

## 💡 Use Cases

### 1. Document Generation Service
Generate multiple files from templates using fragments and configurable placeholders with Base64 content encoding.

### 2. Text Transformation Service
Transform user input on-the-fly for your application.

### 3. Code Generator
Generate code templates, class definitions, configuration files.

### 4. Dynamic Form Builder
Extract form metadata and build UIs dynamically.

### 5. URL Builder
Create safe, encoded URLs with proper query parameters.

### 6. Data Encoder/Decoder
Transform data between Base64, URL encoding, HTML entities.

## 🔌 Integration Examples


### JavaScript
```javascript
// Simple evaluation (plain text)
const response = await fetch('http://localhost:5213/api/qik/evaluate', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    expression: 'camelCase(@input)',
    context: { '@input': 'Hello World' }
  })
});
const data = await response.json();
console.log(data.result); // "helloWorld"

// Document generation (Base64)
const generateResponse = await fetch('http://localhost:5213/api/qik/generate?contentEncoding=base64', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    script: btoa('@className => "User"; @namespace => "MyApp.Models";'),
    fragments: {
      classTemplate: btoa('namespace @{namespace};\n\npublic class @{className}\n{\n}')
    },
    documents: {
      'models/User.cs': '{classTemplate}'
    },
    placeholderPrefix: '@{',
    placeholderSuffix: '}'
  })
});
const generateData = await generateResponse.json();
// Decode Base64 content
Object.entries(generateData.documents).forEach(([path, content]) => {
  console.log(`${path}:\n${atob(content)}`);
});
```


### C#
```csharp
var client = new HttpClient { BaseAddress = new Uri("http://localhost:5213") };

// Simple evaluation (plain text)
var request = new { expression = "upperCase(@name)", context = new { name = "john" } };
var response = await client.PostAsJsonAsync("/api/qik/evaluate", request);
var result = await response.Content.ReadFromJsonAsync<EvaluateExpressionResponse>();

// Document generation (Base64)
var generateRequest = new {
  script = Convert.ToBase64String(Encoding.UTF8.GetBytes("@className => \"User\";")),
  fragments = new Dictionary<string, string> {
    ["classTemplate"] = Convert.ToBase64String(Encoding.UTF8.GetBytes("public class @{className} { }"))
  },
  documents = new Dictionary<string, string> {
    ["User.cs"] = "{classTemplate}"
  },
  placeholderPrefix = "@{",
  placeholderSuffix = "}"
};
var generateResponse = await client.PostAsJsonAsync("/api/qik/generate?contentEncoding=base64", generateRequest);
var generateResult = await generateResponse.Content.ReadFromJsonAsync<GenerateResponse>();
```

### Python
```python
import requests
import base64

# Simple evaluation
result = requests.post('http://localhost:5213/api/qik/evaluate', json={
    'expression': 'camelCase(@input)',
    'context': {'@input': 'hello world'}
}).json()
print(result['result'])  # "helloWorld"

# Document generation
script = base64.b64encode(b'@className => "User";').decode()
template = base64.b64encode(b'public class @{className} { }').decode()

generate_result = requests.post('http://localhost:5213/api/qik/generate', json={
    'script': script,
    'fragments': {'classTemplate': template},
    'documents': {'User.cs': '{classTemplate}'},
    'placeholderPrefix': '@{',
    'placeholderSuffix': '}'
}).json()

for path, content in generate_result['documents'].items():
    print(f"{path}:\n{base64.b64decode(content).decode()}")
```

## 🏗️ Architecture

```
┌─────────────────┐
│   Controllers   │  ← REST Endpoints
├─────────────────┤
│    Services     │  ← Business Logic
├─────────────────┤
│   Qik Library   │  ← Template Engine
└─────────────────┘
```

### Design Patterns
- **Service Layer**: Separates API from business logic
- **DTOs**: Clean API contracts
- **Dependency Injection**: Testable, maintainable code
- **RESTful**: Standard HTTP methods and status codes


### Python
```python
import requests
import base64

# Simple evaluation (plain text)
result = requests.post('http://localhost:5213/api/qik/evaluate', json={
  'expression': 'camelCase(@input)',
  'context': {'@input': 'hello world'}
}).json()
print(result['result'])  # "helloWorld"

# Document generation (Base64)
script = base64.b64encode(b'@className => "User";').decode()
template = base64.b64encode(b'public class @{className} { }').decode()

2. Update base URL if needed
  'script': script,
  'fragments': {'classTemplate': template},
  'documents': {'User.cs': '{classTemplate}'},
  'placeholderPrefix': '@{',
  'placeholderSuffix': '}'
}).json()

for path, content in generate_result['documents'].items():
  print(f"{path}:\n{base64.b64decode(content).decode()}")
```
3. Run requests

### Option 4: cURL
```bash
curl -X POST http://localhost:5213/api/qik/evaluate \
  -H "Content-Type: application/json" \
  -d '{"expression": "guid()"}'
```

## 📦 Project Structure

```
QikApi/
├── Controllers/       # API endpoints
├── Services/          # Business logic
├── Models/            # Request/Response DTOs
├── Program.cs         # App configuration
├── *.md              # Documentation
├── *.http            # Test files
└── *.json            # Postman collection
```

## 🚢 Deployment

The API is a standard ASP.NET Core app and can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
- Kubernetes
- IIS / Nginx
- Self-hosted

## 📊 Performance

- **Stateless**: No session state
- **Thread-safe**: Concurrent requests supported
- **Fast**: < 10ms for simple expressions
- **Memory-efficient**: No state between requests

## 🔐 Security Considerations

For production deployment, consider:
- Authentication/Authorization (JWT, API keys)
- Rate limiting
- Input validation and sanitization
- CORS restrictions
- HTTPS enforcement
- Request size limits

## 🤝 Contributing

Based on the [Qik library](https://github.com/rob-bl8ke/Qik) by rob-bl8ke.

## 📄 License

GPL-3.0-or-later (same as Qik library)

## 🔗 Links

- **Qik Repository**: https://github.com/rob-bl8ke/Qik
- **Qik NuGet**: https://www.nuget.org/packages/rob_bl8ke.Qik/
- **Technical Guide**: `../docs/technical-guide.md`

## 🎓 Learning Path

1. **Start Here**: `GETTING-STARTED.md`
2. **Try Examples**: `example-requests.http`
3. **Explore API**: Swagger UI at http://localhost:5213
4. **Deep Dive**: `../docs/technical-guide.md`
5. **Reference**: `README.md`

## 💬 Example Request Flow

```
Client                    API                      Qik Library
  │                        │                            │
  ├──POST /api/qik/evaluate───────────────►            │
  │  { "expression": "upperCase(@x)",                  │
  │    "context": { "@x": "hello" } }                  │
  │                        │                            │
  │                        ├──Build Script──────────►  │
  │                        │  "@x => \"hello\";         │
  │                        │   @result => upperCase(@x);"
  │                        │                            │
  │                        │                            ├─Parse
  │                        │                            ├─Interpret
  │                        │                            └─Execute
  │                        │                            │
  │                        │◄───Return "HELLO"──────────┤
  │                        │                            │
  │◄────Response────────────┤                           │
  │  { "result": "HELLO",  │                           │
  │    "success": true }   │                           │
```

## 🎉 Success!

You now have a fully functional REST API for the Qik template generation library. Start exploring with the Swagger UI or dive into the example requests!

---

**Built with ❤️ using .NET 9.0 and the Qik template engine**
