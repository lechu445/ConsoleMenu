Purpose
Provide concise, actionable guidance for Copilot/code-agent sessions working in this repository (ConsoleMenu).

Build, test & lint (commands)
- Build solution: dotnet build ConsoleMenu.sln
- Build single project: dotnet build ConsoleMenu\ConsoleMenu.csproj
- Run the sample app (pass menu args after --): dotnet run --project ConsoleMenuSampleApp\ConsoleMenuSampleApp.csproj -- --menu-select="Sub.2"
- Run all tests: dotnet test ConsoleMenu.sln
- Run a single test (by method/class):
  dotnet test ConsoleMenuTests\ConsoleMenuTests.csproj --filter "FullyQualifiedName~ConsoleMenuTests.SimpleScenarioTest.SimpleScenario"
  (use a substring match with FullyQualifiedName to target class/method)
- Pack library for NuGet: dotnet pack ConsoleMenu\ConsoleMenu.csproj -c Release

Linting & analyzers
- StyleCop and Roslyn analyzers are referenced in ConsoleMenu.csproj. Running dotnet build will surface analyzer warnings.
- To treat warnings as errors (fail on StyleCop issues): dotnet build -warnaserror
- stylecop.json is present (ConsoleMenu/stylecop.json) and included as AdditionalFiles; follow rules defined there.

High-level architecture (big picture)
- This repository is a small .NET library (ConsoleMenu) that provides a fluent console menu API.
- Projects:
  - ConsoleMenu: core library (multi-targets netstandard1.3;net6.0). Key types: ConsoleMenu, MenuItem, MenuConfig, ConsoleMenuDisplay, IConsole, SystemConsole.
  - ConsoleMenuSampleApp: minimal sample console app referencing the library (net6.0).
  - ConsoleMenuTests: xUnit test project (net6.0) that uses TestConsole and TestHelpers to simulate console input/output.
- Packaging: the library is configured for NuGet (PackageId: ConsoleMenu-simple). README.md and LICENSE.txt are included in package metadata.
- API stability: PublicAPI.Shipped.txt / PublicAPI.Unshipped.txt are present for tracking the public surface.

Key repo-specific conventions
- Preselected menu items: command-line preselection uses the argument key --menu-select= with a separator (default '.') as shown in README examples. Pass nested selections with quoted strings.
- Fluent API: methods chain (e.g., new ConsoleMenu().Add(...).Configure(...).Show()). Many unit tests rely on injection of a TestConsole via menu.Console for deterministic output.
- Close semantics: use ConsoleMenu.Close to mark a "Close" action or call thisMenu.CloseMenu() inside an item to close programmatically.
- Nullable enabled: projects compile with <Nullable>enable</Nullable>; prefer nullable-correct code when contributing.
- Style & analyzers: StyleCop.Analyzers and Microsoft.CodeAnalysis.PublicApiAnalyzers are included; adhere to existing stylecop.json rules and public API files.
- Versioning/packaging: Version is set in the ConsoleMenu.csproj; packaging includes symbol/snupkg artifacts.

Files to consult quickly
- README.md (root) — usage and examples (preselection, config)
- ConsoleMenu/ConsoleMenu.csproj — targets, analyzers, packaging info
- ConsoleMenu/stylecop.json — StyleCop config
- ConsoleMenu/PublicAPI.*.txt — public API tracking
- ConsoleMenuTests/* — patterns for testing with TestConsole

Other AI assistant configs checked
- No CLAUDE.md, AGENTS.md, .cursorrules, .windsurfrules, or similar files were found.

Notes for Copilot sessions
- Prefer reading README.md and ConsoleMenu/ConsoleMenu.csproj early to understand target frameworks, analyzers, and packaging.
- For behavioral changes, search tests in ConsoleMenuTests to see expected output (they are effective spec examples).
- Use dotnet build/test/pack commands shown above when generating or validating code changes.

MCP servers
- Configured: .github/workflows/mcp-dotnet-tests.yml — a GitHub Actions workflow that runs on windows-latest for push and pull_request. It restores, builds the solution, runs the ConsoleMenuTests project with a TRX logger, and uploads the TestResults.trx as an artifact. Use this workflow as the MCP .NET test runner for CI validations.
- Configured: .github/workflows/mcp-dependency-scan.yml — weekly and on push; runs dotnet list package --vulnerable and fails the job when vulnerable packages are detected. The scan output is uploaded as an artifact (dependency-scan-output).
- Configured: .github/workflows/mcp-sample-app-smoke.yml — runs on Windows for push and PRs; builds the solution, runs the ConsoleMenuSampleApp with a preselected menu argument, asserts the app produced expected text ("Pick an option"), and uploads sample output as an artifact.
- Run tests locally to match the server: dotnet test ConsoleMenuTests/ConsoleMenuTests.csproj --logger "trx;LogFileName=ConsoleMenuTests/TestResults.trx"


Summary
Created .github/copilot-instructions.md with build/test/lint commands, a high-level architecture summary, and repository-specific conventions. Want adjustments or additional coverage (for example, more test-runner examples or CI packaging steps)?
