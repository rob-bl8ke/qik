# VS Code Configuration Overview

Complete VS Code configuration for the Qik solution.

## 📁 Configuration Files

```
.vscode/
├── launch.json                  # Debug configurations
├── tasks.json                   # Build tasks
├── DEBUGGING.md                 # Debug guide (comprehensive)
└── DEBUG-SETUP-SUMMARY.md       # Quick reference
```

## 🎯 Debug Configurations

### 1. QikApi (Web API) ⭐ NEW
**Purpose**: Debug the REST API  
**Target**: `QikApi/bin/Debug/net9.0/QikApi.dll`  
**Features**:
- Auto-builds solution before starting
- Opens browser to Swagger UI automatically
- Sets Development environment
- Port: http://localhost:5213

**Use Cases**:
- Debug API endpoints
- Test request/response handling
- Inspect service layer
- Debug Qik library integration

### 2. .NET Core Launch (console)
**Purpose**: Debug console apps and tests  
**Target**: `QikTests/bin/Debug/net7.0/QikTests.dll`  
**Features**:
- Console application debugging
- Internal console output

**Use Cases**:
- Debug unit tests
- Run console applications

### 3. .NET Core Attach
**Purpose**: Attach to running processes  
**Features**:
- Attach to any running .NET process
- Useful for debugging already-running apps

**Use Cases**:
- Debug running API without restart
- Attach to background services

## ⚙️ Build Tasks

### build
**Command**: `dotnet build Qik.sln`  
**Purpose**: Build entire solution  
**Used by**: All debug configurations (preLaunchTask)

### publish
**Command**: `dotnet publish Qik.sln`  
**Purpose**: Publish solution for deployment

### watch
**Command**: `dotnet watch run --project Qik.sln`  
**Purpose**: Watch for changes and auto-rebuild

## 🚀 Quick Start Guide

### Debug QikApi Web API

```
1. Press F5 (or Run > Start Debugging)
2. Select "QikApi (Web API)" from dropdown
3. Browser opens to http://localhost:5213/swagger
4. Set breakpoints in Controllers/ or Services/
5. Execute requests via Swagger UI
6. Debugger pauses at breakpoints
```

### Debug Tests

```
1. Press F5
2. Select ".NET Core Launch (console)"
3. Or right-click test in Test Explorer > Debug Test
```

### Attach to Running Process

```
1. Start API: dotnet run --project QikApi
2. Press F5
3. Select ".NET Core Attach"
4. Choose QikApi.dll from process list
```

## 🎨 Debug Controls

| Action | Shortcut | Description |
|--------|----------|-------------|
| Start | `F5` | Start debugging |
| Stop | `Shift+F5` | Stop debugging |
| Restart | `Ctrl+Shift+F5` | Restart debugging |
| Step Over | `F10` | Execute line, don't enter methods |
| Step Into | `F11` | Enter method calls |
| Step Out | `Shift+F11` | Exit current method |
| Continue | `F5` | Resume execution |
| Toggle BP | `F9` | Add/remove breakpoint |
| Debug Console | `Ctrl+Shift+Y` | Open debug console |

## 📊 Project Structure

```
qik/                                    (Workspace root)
├── .vscode/
│   ├── launch.json                    # You are here
│   ├── tasks.json                     # Build tasks
│   ├── DEBUGGING.md                   # Full guide
│   └── DEBUG-SETUP-SUMMARY.md         # Quick ref
│
├── Qik/                               # Core library
│   └── bin/Debug/net7.0/
│
├── QikApi/                            # REST API ⭐
│   ├── bin/Debug/net9.0/              # Debug target
│   │   └── QikApi.dll                 # Debugged DLL
│   ├── Controllers/                   # Set breakpoints
│   │   └── QikController.cs
│   ├── Services/                      # Business logic
│   │   └── QikService.cs
│   └── Properties/
│       └── launchSettings.json        # Launch profiles
│
├── QikTests/                          # Unit tests
│   └── bin/Debug/net7.0/
│
└── QikApiTests/                       # API tests
    └── bin/Debug/net9.0/
```

## 🔧 Configuration Details

### launch.json - QikApi Configuration

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

### Key Properties

| Property | Value | Purpose |
|----------|-------|---------|
| `name` | "QikApi (Web API)" | Display name in dropdown |
| `program` | Path to QikApi.dll | DLL to debug |
| `cwd` | QikApi folder | Working directory |
| `preLaunchTask` | "build" | Build solution first |
| `serverReadyAction` | Open Swagger | Auto-launch browser |
| `ASPNETCORE_ENVIRONMENT` | Development | Enable dev features |

## 🌐 URLs and Endpoints

| URL | Purpose |
|-----|---------|
| http://localhost:5213 | API base URL |
| http://localhost:5213/swagger | Swagger UI (auto-opens) |
| http://localhost:5213/api/qik/health | Health check |
| http://localhost:5213/api/qik/interpret | Interpret script |
| http://localhost:5213/api/qik/evaluate | Evaluate expression |
| http://localhost:5213/api/qik/widgets | Extract widgets |
| http://localhost:5213/api/qik/functions | List functions |

## 🐛 Common Debug Scenarios

### Debug a Controller Endpoint

```csharp
// QikController.cs
[HttpPost("interpret")]
public ActionResult<InterpretResponse> Interpret([FromBody] InterpretRequest request)
{
    _logger.LogInformation("Interpreting Qik script"); // <- Breakpoint
    
    var response = _qikService.Interpret(request);
    
    if (!response.Success)
    {
        return BadRequest(response); // <- Breakpoint
    }
    
    return Ok(response);
}
```

### Debug Service Logic

```csharp
// QikService.cs
public InterpretResponse Interpret(InterpretRequest request)
{
    try
    {
        var terminal = _interpreter.Interpret(_functionFactory, request.Script); // <- Breakpoint
        
        var values = new Dictionary<string, string>();
        foreach (var symbol in terminal.Symbols)
        {
            values[symbol] = terminal.GetValue(symbol); // <- Breakpoint
        }
        
        return new InterpretResponse { Success = true, Values = values };
    }
    catch (Exception ex)
    {
        return new InterpretResponse { Success = false, ErrorMessage = ex.Message }; // <- Breakpoint
    }
}
```

### Debug Request/Response

1. Set breakpoint at controller method entry
2. Inspect `request` object in Debug Console
3. Step through validation logic
4. Watch `response` object being built
5. Check HTTP status code in return statement

## 📈 Debug Workflow

```
1. Open VS Code → Solution root
2. Set Breakpoints → Controllers, Services, etc.
3. Press F5 → Start debugging
4. Select Config → "QikApi (Web API)"
5. Browser Opens → Swagger UI appears
6. Execute Request → Via Swagger
7. Debugger Pauses → At your breakpoint
8. Inspect Variables → Watch, Debug Console
9. Step Through → F10, F11
10. Continue → F5 to finish
```

## 💡 Pro Tips

### Hot Reload
- Modify code while debugging
- Many changes apply without restart
- Save file to trigger reload

### Debug Console
Type C# expressions while paused:
```csharp
request.Script
response.Values.Count
terminal.Symbols
_qikService.GetAvailableFunctions()
```

### Conditional Breakpoints
- Right-click breakpoint
- Add condition: `request.Script.Contains("error")`
- Or hit count: `>= 5`

### Multiple Debugging
- Start API with F5
- Attach to another process with "Attach" config
- Debug multiple services simultaneously

### Logpoints
- Right-click line number
- Add Logpoint instead of breakpoint
- Logs message without pausing

## 🔍 Troubleshooting

### Port in Use
```powershell
# Check what's using the port
netstat -ano | findstr :5213

# Kill the process
taskkill /PID <process_id> /F
```

### Breakpoint Not Hit
- Verify build is up to date
- Check Debug vs Release configuration
- Ensure code is actually being executed

### Can't Launch Browser
- Check firewall settings
- Verify launchSettings.json URLs
- Try manual navigation to http://localhost:5213/swagger

### Build Fails
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

## 📚 Documentation

| File | Purpose |
|------|---------|
| `DEBUGGING.md` | Comprehensive debugging guide |
| `DEBUG-SETUP-SUMMARY.md` | Quick reference |
| `../QikApi/README.md` | API documentation |
| `../QikApi/OVERVIEW.md` | API overview |

## ✅ Status

All configurations are ready to use:

- ✅ QikApi debug configuration
- ✅ Build tasks configured
- ✅ Browser auto-launch enabled
- ✅ Environment variables set
- ✅ Documentation complete

**Ready to debug! Press F5!** 🎯

## 🎓 Learning Resources

- [VS Code C# Debugging](https://code.visualstudio.com/docs/csharp/debugging)
- [ASP.NET Core Debugging](https://docs.microsoft.com/en-us/aspnet/core/test/debug-aspnetcore-apps)
- [.NET Debugging Guide](https://github.com/dotnet/vscode-csharp/blob/main/debugger-launchjson.md)

---

**Quick Start**: Press `F5`, select "QikApi (Web API)", and start debugging! 🚀
