using ConsoleMenuTests.TestHelpers;
using ConsoleTools;
using System;
using Xunit;

namespace ConsoleMenuTests
{
  public class FilteringScenarioTest
  {
    [Fact]
    public void Filtering_Navigation()
    {
      var console = new TestConsole();
      console.AddUserInputWithActionBefore("e", () => AssertHelper.Equal(@"Pick an option:
>> [0] One
   [1] Two
   [2] Close
   [3] Exit
Filter: ", console.ToString()));

      console.AddUserInputWithActionBefore(((char)ConsoleKey.DownArrow).ToString(), () => AssertHelper.Equal(@"Pick an option:
>> [0] One
   [2] Close
   [3] Exit
Filter: e", console.ToString()));

      console.AddUserInputWithActionBefore(((char)ConsoleKey.UpArrow).ToString(), () => AssertHelper.Equal(@"Pick an option:
   [0] One
>> [2] Close
   [3] Exit
Filter: e", console.ToString()));

      console.AddUserInputWithActionBefore(((char)ConsoleKey.UpArrow).ToString(), () => AssertHelper.Equal(@"Pick an option:
>> [0] One
   [2] Close
   [3] Exit
Filter: e", console.ToString()));

      console.AddUserInputWithActionBefore(((char)ConsoleKey.UpArrow).ToString(), () => AssertHelper.Equal(@"Pick an option:
   [0] One
   [2] Close
>> [3] Exit
Filter: e", console.ToString()));

      console.AddUserInputWithActionBefore(((char)ConsoleKey.Enter).ToString(), () => AssertHelper.Equal(@"Pick an option:
   [0] One
>> [2] Close
   [3] Exit
Filter: e", console.ToString()));

      var submenu = new ConsoleMenu { console = console }
        .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
        m.EnableFilter = true;
      });

      var menu = new ConsoleMenu() { console = console }
      .Add("One", () => { })
      .Add("Two", submenu.Show)
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
        m.EnableFilter = true;
      });
      menu.Show();

      AssertHelper.Equal(@"Pick an option:
   [0] One
>> [2] Close
   [3] Exit
Filter: e
", console.ToString());
    }
  }
}
