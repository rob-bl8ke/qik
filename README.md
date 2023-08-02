
# Get up and Running

Find it on [Nuget](https://www.nuget.org/packages/rob_bl8ke.Qik/)

### Run
To run the console application only the `dotnet run` command is necessary unless running for the first time.

```bash
dotnet clean
dotnet restore
dotnet build
```

To enure a clean restart when restoring the cache (or if you run into dependency issues), you an try: `dotnet restore --no-cache`.

```
dotnet publish -c release
```
# Tests

To run the tests simply run the following commands.
```bash
dotnet test
```

```bash
dotnet test ./qiktests/qiktests.csproj
```
### Internals Visible

Adding `InternalsVisibleTo` for tests.
```xml
  <ItemGroup>
    <InternalsVisibleTo Include="QikTests" /> <!-- [assembly: InternalsVisibleTo("CustomTest1")] -->
  </ItemGroup>
```

### Possible Test Explorers/Runners

It might be worth exploring these and others like them at a later stage. At this point you haven't found any of these working effectively.

- https://marketplace.visualstudio.com/items?itemName=hbenl.vscode-test-explorer&ssr=false#overview
- https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer&ssr=false#overview
- https://marketplace.visualstudio.com/items?itemName=wghats.vscode-nxunit-test-adapter&ssr=false#overview

# Maintenance

To see the current status of your installed SDKs and whether there are patches or updates available run `dotnet sdk check`. 

### Version Management

To target an SDK (or SDK range), `global.json` is used:

```json
{
    "sdk": {
        "version": "7.0.203",
        "rollForward": "latestFeature"
    }
}
```
This specifies that the the latest installed "7.0.*" can be used.

When a major version is changed (eg. net6 to net7), the following should be checked and modified:

- `release.sh` needs to target the correct folder.
- `launch.json` must be modified in all places where the new build folders are specified.
- `QikConsoleTests.csproj` must be modified where `BuildAndCopyTestPlugin` is described.


# Antlr

### ANTLR4 grammar syntax support VS Code Plugin

The extension for ANTLR4 support in Visual Studio code. Provides Code Completion + Symbol Information, Grammar Validations, and Visualizations.

- ANTLR4 grammar syntax support [MarketPlace](https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4&ssr=false#qna)
- ANTLR4 grammar syntax support [Github](https://github.com/mike-lischke/vscode-antlr4)

### Usage

Important that the settings are set up correctly or the grammar file will not generate into C# source code. The mode must be external in order to use the CSharp option and it is important to set the output directory and namespace using the item keys below:

 Item | Value |
| --- | :--- |
| mode | external  |
| language | CSharp  |
| listeners | true  |
| visitors | true  |
| outputDir | _antlr  |
| package | CygSoft.Qik.Antlr  |

When everything is working the files in the `./QikAntlr/_antlr` folder will generate every time a change is made to the `QikTemplate.g4` file. If your files aren't generating it is usually because of one of the reasons below:

- Ensuring that you've added the correcdt settings above for both user and workspace.
- It is possible that there is a problem with your `*.g4` template file.

### Plugin Workspace Settings
```
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
