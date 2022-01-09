using System;
using ConsoleMenuTests.TestHelpers;
using ConsoleTools;
using Xunit;

namespace ConsoleMenuTests
{
  public class SimpleScenarioTest
  {
    [Fact]
    public void SimpleScenario()
    {
      var console = new TestConsole();
      console.AddUserInput("2");

      var menu = new ConsoleMenu() { Console = console }
      .Add("One", () => { })
      .Add("Two", () => { })
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });
      menu.Show();

      Assert.Equal(@"Pick an option:
   [0] One
   [1] Two
>> [2] Close
   [3] Exit

", console.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void SimpleScenario_Colors()
    {
      var console = new TestConsole { Details = true };
      console.AddUserInput("2");

      var menu = new ConsoleMenu() { Console = console }
      .Add("One", () => { })
      .Add("Two", () => { })
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });
      menu.Show();

      Assert.Equal(@"Pick an option:<fc: White, bc: Black>
   <fc: White, bc: Black>[0] One<fc: White, bc: Black>
   <fc: White, bc: Black>[1] Two<fc: White, bc: Black>
>> <fc: Black, bc: White>[2] Close<fc: Black, bc: White>
   <fc: White, bc: Black>[3] Exit<fc: White, bc: Black>

", console.ToString(), ignoreLineEndingDifferences: true);
    }
  }
}
