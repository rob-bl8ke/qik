# Generate Endpoint Implementation Summary

## New API Endpoint Added

### `POST /api/qik/generate`

**Purpose**: Generates output using a Qik script, definition template, and fragment variables.

## Request Structure

```json
{
  "script": "base64-encoded-qik-script (required)",
  "fragments": {
    "fragmentKey1": "base64-encoded-content",
    "fragmentKey2": "base64-encoded-content"
  },
  "documents": {
    "documentPath1": "fragment-references",
    "documentPath2": "fragment-references"
  },
  "inputs": {
    "@variable1": "value1",
    "@variable2": "value2"
  }
}
```

## Response Structure

```json
{
  "success": true,
  "documents": {
    "src/models/User.cs": "base64-encoded-generated-content",
    "src/controllers/UserController.cs": "base64-encoded-generated-content"
  },
  "errorMessage": null,
  "metadata": {
    "fragmentCount": 3,
    "documentCount": 2,
    "inputCount": 2,
    "symbolCount": 4
  }
}
```

## Implementation Details

### New Models Created
- `GenerateRequest.cs` - Request DTO with Script, Definition, and Fragments
- `GenerateResponse.cs` - Response DTO with Success, Output, ErrorMessage, and Metadata

### Service Layer
- Added `Generate()` method to `IQikService` interface
- Implemented `Generate()` method in `QikService` class
- Combines script with definition to create output
- Applies fragment values to InputSymbols where possible
- Handles KeyNotFoundException for fragments that don't match InputSymbols

### Controller Layer
- Added `[HttpPost("generate")]` endpoint to `QikController`
- Full validation and error handling
- Swagger documentation included
- Consistent with existing endpoint patterns

## How It Works

1. **Decode Script**: Decodes the base64 Qik script to establish variable context
2. **Interpret Script**: Parses the decoded Qik script and applies any input variables
3. **Process Fragments**: 
   - Decodes base64 fragment content
   - Replaces placeholders (`@{variable}`) with values from interpreted script variables
4. **Build Documents**: 
   - Combines processed fragments based on document template references
   - Each document specifies which fragments to include and how to combine them
5. **Return Results**: Returns generated documents with file paths as keys and Base64 encoded content as values

## Example Usage

```http
POST /api/qik/generate
Content-Type: application/json

{
  "script": "QG5hbWUgPT4gIkpvaG4iOyBAYWdlID0+ICIzMCI7", // base64: @name => "John"; @age => "30";
  "fragments": {
    "classTemplate": "cHVibGljIGNsYXNzIEB7bmFtZX0geyB9", // base64: public class @{name} { }
    "greeting": "SGVsbG8gQHtuYW1lfSwgeW91IGFyZSBAe2FnZX0geWVhcnMgb2xkIQ==" // base64: Hello @{name}, you are @{age} years old!
  },
  "documents": {
    "src/models/Person.cs": "{classTemplate}",
    "output/greeting.txt": "{greeting}"
  },
  "inputs": {
    "@name": "Jane",
    "@age": "25"
  }
}
```

**Response:**
```json
{
  "success": true,
  "documents": {
    "src/models/Person.cs": "cHVibGljIGNsYXNzIEphbmUgeyB9", // base64: "public class Jane { }"
    "output/greeting.txt": "SGVsbG8gSmFuZSwgeW91IGFyZSAyNSB5ZWFycyBvbGQh" // base64: "Hello Jane, you are 25 years old!"
  },
  "metadata": {
    "fragmentCount": 2,
    "documentCount": 2,
    "inputCount": 2,
    "symbolCount": 2
  }
}
```

## Error Handling

- Validates required fields (script, definition)
- Returns 400 Bad Request for validation errors
- Handles Qik interpretation exceptions
- Gracefully handles fragment keys that don't match InputSymbols

## Build Status

✅ **All projects compile successfully**
✅ **Solution builds without errors**
✅ **Ready for testing and deployment**

---

The generate endpoint is now fully implemented and ready to use! 🚀