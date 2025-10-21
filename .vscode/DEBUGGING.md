# Debugging QikApi in VS Code

This document explains how to debug the QikApi Web API project in Visual Studio Code.

## Quick Start

1. **Open the project** in VS Code (the root `qik` folder)
2. **Set breakpoints** in your code (e.g., in `Controllers/QikController.cs`)
3. **Press F5** or select "Run > Start Debugging"
4. **Choose "QikApi (Web API)"** from the debug configuration dropdown
5. **Browser opens automatically** to Swagger UI at `http://localhost:5213/swagger`

## Debug Configuration

The debug configuration is located in `.vscode/launch.json`:

```json
{
    "name": "QikApi (Web API)",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/QikApi/bin/Debug/net9.0/QikApi.dll",
    "args": [],
    "cwd": "${workspaceFolder}/QikApi",
    "stopAtEntry": false,
    "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "%s/swagger"
    },
    "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
    }
}
```

## Key Features

### Automatic Build
- The `preLaunchTask: "build"` ensures the entire solution is built before debugging
- Any compilation errors will be shown before the debugger starts

### Automatic Browser Launch
- The `serverReadyAction` automatically opens your browser when the API starts
- Opens directly to the Swagger UI at `/swagger` for easy testing
- You can change `uriFormat` to open a different URL if needed

### Development Environment
- Sets `ASPNETCORE_ENVIRONMENT=Development` for development features
- Enables detailed error pages
- Loads development-specific configuration from `appsettings.Development.json`

## Debugging Workflow

### 1. Setting Breakpoints

Click in the gutter (left of line numbers) to set breakpoints:

**Controller Example:**
```csharp
[HttpPost("interpret")]
public ActionResult<InterpretResponse> Interpret([FromBody] InterpretRequest request)
{
    _logger.LogInformation("Interpreting Qik script"); // <- Set breakpoint here
    
    var response = _qikService.Interpret(request); // <- Or here
    return Ok(response);
}
```

**Service Example:**
```csharp
public InterpretResponse Interpret(InterpretRequest request)
{
    var terminal = _interpreter.Interpret(_functionFactory, request.Script); // <- Breakpoint here
    
    foreach (var symbol in terminal.Symbols)
    {
        values[symbol] = terminal.GetValue(symbol); // <- Or here
    }
}
```

### 2. Start Debugging

**Option A: Keyboard**
- Press `F5`

**Option B: Command Palette**
- `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (Mac)
- Type "Debug: Start Debugging"

**Option C: Debug View**
- Click the Debug icon in the Activity Bar (left sidebar)
- Select "QikApi (Web API)" from the dropdown
- Click the green play button

### 3. Testing API Endpoints

Once the browser opens with Swagger:

1. **Expand an endpoint** (e.g., `POST /api/qik/interpret`)
2. **Click "Try it out"**
3. **Edit the request body**
4. **Click "Execute"**
5. **Your breakpoint will be hit** in VS Code

### 4. Debugging Controls

While debugging:

| Action | Keyboard | Description |
|--------|----------|-------------|
| Continue | `F5` | Resume execution |
| Step Over | `F10` | Execute current line |
| Step Into | `F11` | Step into method |
| Step Out | `Shift+F11` | Step out of method |
| Restart | `Ctrl+Shift+F5` | Restart debugging |
| Stop | `Shift+F5` | Stop debugging |

### 5. Debug Console

Use the Debug Console to:
- **Evaluate expressions**: Type any C# expression
- **Inspect variables**: See current values
- **Call methods**: Execute code while paused

Example expressions:
```csharp
request.Script
result.Values["@name"]
terminal.Symbols.Length
_qikService.GetAvailableFunctions()
```

## Common Debugging Scenarios

### Debug a Specific Endpoint

1. Set breakpoint in the controller method
2. Start debugging (F5)
3. Use Swagger to call the endpoint
4. Debugger pauses at your breakpoint

### Debug Request Validation

```csharp
[HttpPost("interpret")]
public ActionResult<InterpretResponse> Interpret([FromBody] InterpretRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Script)) // <- Breakpoint here
    {
        return BadRequest(new { error = "Script cannot be empty" });
    }
}
```

### Debug Service Logic

```csharp
public InterpretResponse Interpret(InterpretRequest request)
{
    try
    {
        var terminal = _interpreter.Interpret(_functionFactory, request.Script); // <- Breakpoint
        
        // Inspect terminal.Symbols in Debug Console
        // Inspect terminal.GetValue("@variableName")
    }
    catch (Exception ex)
    {
        // Breakpoint here to inspect exceptions
    }
}
```

### Debug Qik Library Interaction

```csharp
// Step into Qik library calls
var terminal = _interpreter.Interpret(_functionFactory, request.Script); // <- F11 to step in

// Inspect intermediate values
foreach (var symbol in terminal.Symbols)
{
    var value = terminal.GetValue(symbol); // <- Check each value
}
```

## Watch Window

Add variables to the Watch window to monitor them:

1. Right-click a variable → "Add to Watch"
2. Or manually add expressions in the Watch panel

Useful watches:
```
request
request.Script
result.Success
result.Values
terminal.Symbols
_functionFactory
```

## Conditional Breakpoints

Right-click a breakpoint → "Edit Breakpoint" to add conditions:

**Expression:**
```csharp
request.Script.Contains("error")
```

**Hit Count:**
```
>= 5  // Break on 5th hit or later
```

## Debugging with Tests

To debug tests instead:

1. Select ".NET Core Launch (console)" configuration
2. Or right-click a test in Test Explorer → "Debug Test"

## Debugging Tips

### Hot Reload
.NET 9.0 supports hot reload - many changes apply without restarting:
- Modify method bodies
- Add/remove methods (with some limitations)
- Watch the Debug Console for hot reload status

### Launch Profiles
The API uses launch profiles from `Properties/launchSettings.json`:
- Development profile is used when debugging
- Can customize URLs, environment variables, etc.

### HTTPS Development Certificate
If you see HTTPS certificate warnings:
```powershell
dotnet dev-certs https --trust
```

### Multiple Instances
To debug multiple processes:
1. Start first instance with F5
2. Start second instance from terminal
3. Use "Attach to Process" for the second

## Troubleshooting

### Port Already in Use
If port 5213 is in use:
1. Check `Properties/launchSettings.json`
2. Modify the `applicationUrl`
3. Or kill the process using the port

### Breakpoint Not Hit
- Ensure code is built (check timestamp on DLL)
- Verify breakpoint is in executable code (not comments/declarations)
- Check that you're debugging the right configuration

### Can't Step Into Qik Library
- Qik library doesn't have debug symbols by default
- You can build Qik in Debug mode with symbols if needed

### Changes Not Reflected
- Stop debugging
- Clean solution: `dotnet clean`
- Rebuild: `dotnet build`
- Start debugging again

## Advanced Scenarios

### Attach to Running Process
1. Start API without debugger: `dotnet run --project QikApi`
2. In VS Code: Select ".NET Core Attach" configuration
3. Choose the `QikApi` process from the list

### Remote Debugging
For debugging on remote servers, see:
https://code.visualstudio.com/docs/csharp/debugging

### Debugging Background Services
If you add background services to QikApi:
- Set breakpoints in hosted service methods
- Use cancellation token breakpoints
- Monitor service lifetime in logs

## Related Files

- `.vscode/launch.json` - Debug configurations
- `.vscode/tasks.json` - Build tasks
- `QikApi/Properties/launchSettings.json` - Launch profiles
- `QikApi/appsettings.Development.json` - Dev settings

## Additional Resources

- [VS Code C# Debugging](https://code.visualstudio.com/docs/csharp/debugging)
- [ASP.NET Core Debugging](https://docs.microsoft.com/en-us/aspnet/core/test/debug-aspnetcore-apps)
- [.NET Debugging in VS Code](https://github.com/dotnet/vscode-csharp/blob/main/debugger-launchjson.md)

## Quick Reference

| Task | Action |
|------|--------|
| Start Debugging | `F5` |
| Stop Debugging | `Shift+F5` |
| Restart | `Ctrl+Shift+F5` |
| Step Over | `F10` |
| Step Into | `F11` |
| Step Out | `Shift+F11` |
| Toggle Breakpoint | `F9` |
| Open Debug Console | `Ctrl+Shift+Y` |
| Show Debug View | `Ctrl+Shift+D` |

---

Happy Debugging! 🐛🔍
