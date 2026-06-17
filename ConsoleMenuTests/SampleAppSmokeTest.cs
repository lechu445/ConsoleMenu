using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleMenuTests
{
  public class SampleAppSmokeTest
  {
    [Fact(Skip = "Skipping due to flaky behavior")]
    public async Task SampleAppProducesMenuText()
    {
      var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "ConsoleMenuSampleApp", "ConsoleMenuSampleApp.csproj"));

      var start = new ProcessStartInfo
      {
        FileName = "dotnet",
        Arguments = $"run --project \"{projectPath}\" -- --menu-select=\"5\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = Path.GetFullPath("..\\")
      };

      using var proc = Process.Start(start);
      if (proc == null) throw new InvalidOperationException("Failed to start dotnet process");

      var output = await proc.StandardOutput.ReadToEndAsync();
      var err = await proc.StandardError.ReadToEndAsync();

      if (!proc.WaitForExit(15000))
      {
        proc.Kill();
        throw new TimeoutException("Sample app did not exit in time");
      }

      Assert.Contains("Pick an option", output + err);
    }
  }
}
