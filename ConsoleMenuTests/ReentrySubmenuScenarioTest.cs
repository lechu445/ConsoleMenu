using System;
using ConsoleMenuTests.TestHelpers;
using ConsoleTools;
using Xunit;

namespace ConsoleMenuTests
{
  public class ReentrySubmenuScenarioTest
  {
    [Fact]
    public void Reentry_Submenu()
    {
      var console = new TestConsole();
      console.AddUserInput(ConsoleKey.D1);
      console.AddUserInputWithActionBefore(ConsoleKey.D1, () =>
      {
        AssertHelper.Equal(@"Pick an option:
   [0] One
>> [1] Close
", console.ToString());
      });

      // open submenu once again
      console.AddUserInputWithActionBefore(ConsoleKey.D1, () =>
      {
        AssertHelper.Equal(@"Pick an option:
   [0] Sub_One
>> [1] Sub_Close
", console.ToString());
      });
      console.AddUserInputWithActionBefore(ConsoleKey.D1, () =>
      {
        AssertHelper.Equal(@"Pick an option:
   [0] One
>> [1] Close
", console.ToString());
      });
      var submenu = new ConsoleMenu() { Console = console }
      .Add("Sub_One", () => { })
      .Add("Sub_Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });

      var menu = new ConsoleMenu() { Console = console }
      .Add("One", submenu.Show)
      .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });
      menu.Show();

      AssertHelper.Equal(@"Pick an option:
   [0] One
>> [1] Close

", console.ToString());
    }
  }
}
