using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleMenuTests
{
  public class SampleAppSmokeTest
  {
    [Fact]
    public async Task SampleAppProducesMenuText()
    {
      var start = new ProcessStartInfo
      {
        FileName = "dotnet",
        Arguments = "run --project ../ConsoleMenuSampleApp/ConsoleMenuSampleApp.csproj -- --menu-select=\"0\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = Path.GetFullPath("..\")
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
