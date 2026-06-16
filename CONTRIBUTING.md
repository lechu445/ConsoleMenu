Contributing

Thanks for considering contributing to ConsoleMenu. Quick guidelines to get changes accepted:

- Run tests locally: dotnet test ConsoleMenuTests/ConsoleMenuTests.csproj
- Follow nullable-enabled conventions (<Nullable>enable</Nullable>) and fix warnings where practical.
- Keep public API changes reflected in ConsoleMenu/PublicAPI.Unshipped.txt and run the maintainers' API review process.
- Code style: StyleCop analyzers are enabled — run dotnet build to see analyzer warnings. Prefer to address warnings before opening PRs.

Pull requests
- Base PRs against main or master.
- Describe the change, include before/after snippets for behavioral changes, and reference related tests.

Local sample app
- Run the sample app for manual verification: dotnet run --project ConsoleMenuSampleApp/ConsoleMenuSampleApp.csproj -- --menu-select="0"

Thanks — maintainers will review and provide guidance.
