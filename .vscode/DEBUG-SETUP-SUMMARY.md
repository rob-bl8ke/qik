# QikApi Debug Configuration - Summary

## ✅ Configuration Complete

The QikApi Web API project is now fully configured for debugging in VS Code!

## 📋 What Was Added

### 1. Debug Configuration (`.vscode/launch.json`)

Added a new launch configuration: **"QikApi (Web API)"**

```json
{
    "name": "QikApi (Web API)",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/QikApi/bin/Debug/net9.0/QikApi.dll",
    "cwd": "${workspaceFolder}/QikApi",
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

### 2. Key Features

✅ **Automatic Build** - Solution builds before debugging  
✅ **Automatic Browser Launch** - Opens Swagger UI when API starts  
✅ **Development Environment** - Sets proper environment variables  
✅ **Correct Paths** - Points to .NET 9.0 build output  
✅ **Working Directory** - Set to QikApi project folder  

### 3. Documentation

Created **`.vscode/DEBUGGING.md`** with comprehensive debugging guide:
- Quick start instructions
- Setting breakpoints
- Debugging workflow
- Common scenarios
- Troubleshooting tips
- Keyboard shortcuts

## 🚀 How to Use

### Quick Start

1. **Open VS Code** at the solution root (`qik` folder)
2. **Press F5** or click the Debug icon
3. **Select "QikApi (Web API)"** from the dropdown
4. **Browser opens** automatically to Swagger UI
5. **Test endpoints** - breakpoints will be hit!

### Debug a Request

1. Set breakpoint in `Controllers/QikController.cs`:
   ```csharp
   public ActionResult<InterpretResponse> Interpret([FromBody] InterpretRequest request)
   {
       _logger.LogInformation("Interpreting Qik script"); // <- Breakpoint here
   ```

2. Start debugging (F5)

3. In Swagger UI:
   - Expand `POST /api/qik/interpret`
   - Click "Try it out"
   - Click "Execute"

4. VS Code pauses at your breakpoint!

## 🎯 Configuration Details

### Project Structure
```
qik/                              (Workspace root)
├── .vscode/
│   ├── launch.json              (Debug configurations)
│   ├── tasks.json               (Build tasks)
│   └── DEBUGGING.md             (Documentation)
├── QikApi/
│   ├── bin/Debug/net9.0/        (Build output - debug target)
│   ├── Controllers/             (Set breakpoints here)
│   ├── Services/                (Or here)
│   └── Properties/
│       └── launchSettings.json  (Launch profiles)
```

### URLs and Ports

| Profile | URL | Notes |
|---------|-----|-------|
| HTTP | http://localhost:5213 | Used by debugger |
| HTTPS | https://localhost:7212 | Alternative (requires cert) |
| Swagger | http://localhost:5213/swagger | Auto-opens in browser |

### Environment

- **ASPNETCORE_ENVIRONMENT**: Development
- **Config File**: `appsettings.Development.json`
- **Build Configuration**: Debug
- **Target Framework**: net9.0

## 🎨 Debug View

Available debug configurations:

1. **QikApi (Web API)** ⭐ - Debug the REST API
2. **.NET Core Launch (console)** - Debug QikTests
3. **.NET Core Attach** - Attach to running process

## ⚡ Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Start Debugging | `F5` |
| Stop Debugging | `Shift+F5` |
| Restart | `Ctrl+Shift+F5` |
| Step Over | `F10` |
| Step Into | `F11` |
| Step Out | `Shift+F11` |
| Toggle Breakpoint | `F9` |

## 🔍 What You Can Debug

### Controllers
- Request handling
- Input validation
- Response generation
- HTTP status codes

### Services
- Business logic
- Qik interpretation
- Widget extraction
- Expression evaluation

### Qik Library Integration
- Script parsing
- Function execution
- Variable resolution
- Error handling

## 📊 Debug Workflow Example

1. **Set Breakpoint** in `QikController.Interpret()`
2. **Press F5** - API starts, browser opens
3. **Use Swagger** - Execute POST request
4. **Debugger Pauses** - Inspect request object
5. **Step Through** (F10) - Watch execution
6. **Check Variables** - Use Debug Console
7. **Continue** (F5) - See response in Swagger

## ✅ Verification

Test the configuration:

```powershell
# 1. Open VS Code at solution root
cd c:\Code\rob-bl8ke\qik

# 2. Open in VS Code
code .

# 3. Press F5, select "QikApi (Web API)"
# 4. Browser should open to http://localhost:5213/swagger
# 5. Set a breakpoint in QikController
# 6. Execute a request in Swagger
# 7. Breakpoint should be hit!
```

## 📚 Additional Resources

- **Full Guide**: `.vscode/DEBUGGING.md`
- **API Documentation**: `QikApi/README.md`
- **VS Code Docs**: https://code.visualstudio.com/docs/csharp/debugging

## 🎉 Status

✅ Debug configuration created  
✅ Paths verified (bin/Debug/net9.0)  
✅ Build task configured  
✅ Browser auto-launch enabled  
✅ Environment variables set  
✅ Documentation complete  

**Ready to debug! Press F5 to start!** 🚀
