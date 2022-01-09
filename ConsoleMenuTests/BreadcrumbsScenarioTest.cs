using System;
using ConsoleMenuTests.TestHelpers;
using ConsoleTools;
using Xunit;

namespace ConsoleMenuTests
{
  public class BreadcrumbsScenarioTest
  {
    [Fact]
    public void Breadcrumbs()
    {
      var console = new TestConsole();
      console.AddUserInputWithActionBefore("1", () => AssertHelper.Equal(@"First menu
Pick an option:
>> [0] One
   [1] Two
   [2] Close
   [3] Exit
", console.ToString()));

      console.AddUserInputWithActionBefore("0", () => AssertHelper.Equal(@"First menu > Second menu
Pick an option:
>> [0] Close
", console.ToString()));

      console.AddUserInputWithActionBefore("2", () => AssertHelper.Equal(@"First menu
Pick an option:
   [0] One
>> [1] Two
   [2] Close
   [3] Exit
", console.ToString()));

      var submenu = new ConsoleMenu { Console = console }
        .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
        m.EnableBreadcrumb = true;
        m.WriteBreadcrumbAction = titles => console.WriteLine(string.Join(" > ", titles));
        m.Title = "Second menu";
      });

      var menu = new ConsoleMenu() { Console = console }
      .Add("One", () => { })
      .Add("Two", submenu.Show)
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
        m.EnableBreadcrumb = true;
        m.WriteBreadcrumbAction = titles => console.WriteLine(string.Join(" > ", titles));
        m.Title = "First menu";
      });
      menu.Show();

      AssertHelper.Equal(@"First menu
Pick an option:
   [0] One
   [1] Two
>> [2] Close
   [3] Exit

", console.ToString());
    }
  }
}
