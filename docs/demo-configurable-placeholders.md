# Configurable Placeholder Format Demo

## What we've implemented

The `GenerateRequest` now supports configurable placeholder formats through two new properties:

- `PlaceholderPrefix`: Default value `"@{"`
- `PlaceholderSuffix`: Default value `"}"`

This allows you to use different placeholder formats in your templates:

## Examples

### Default format (@{variable})
```json
{
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```
Template: `Hello @{name}, welcome to @{company}!`

### Custom format (${variable})
```json
{
  "placeholderPrefix": "${",
  "placeholderSuffix": "}"
}
```
Template: `Hello ${name}, welcome to ${company}!`

### Mustache-style format ({{variable}})
```json
{
  "placeholderPrefix": "{{",
  "placeholderSuffix": "}}"
}
```
Template: `Hello {{name}}, welcome to {{company}}!`

### Angular-style format ({{variable}})
```json
{
  "placeholderPrefix": "[[",
  "placeholderSuffix": "]]"
}
```
Template: `Hello [[name]], welcome to [[company]]!`

## Updated Postman Collection

All three test cases in the Postman collection now include:
```json
{
  "placeholderPrefix": "@{",
  "placeholderSuffix": "}"
}
```

## Technical Changes Made

1. **GenerateRequest.cs**: Added `PlaceholderPrefix` and `PlaceholderSuffix` properties with defaults
2. **QikService.cs**: Updated `ReplacePlaceholders` method to accept and use configurable prefix/suffix
3. **QikController.cs**: Updated documentation to mention configurable placeholder format
4. **Postman Collection**: Added placeholder format properties to all generate test cases

## Benefits

- **Flexibility**: Support any placeholder format your templates require
- **Compatibility**: Existing templates continue to work with default `@{variable}` format
- **Versatility**: Can integrate with existing template systems that use different placeholder styles
- **Future-proof**: Easy to adapt to new placeholder requirements

## How it works

The `ReplacePlaceholders` method now:
1. Takes the prefix and suffix from the request
2. Builds the complete placeholder pattern: `{prefix}variableName{suffix}`
3. Replaces all occurrences with values from the interpreted Qik script
4. Handles both `@variable` and `variable` symbol names automatically

To test this once the API is restarted, you can modify the `placeholderPrefix` and `placeholderSuffix` in any of the Postman test cases and update the corresponding Base64 encoded fragments to use your preferred placeholder format.