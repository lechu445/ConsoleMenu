using System;
using ConsoleMenuTests.TestHelpers;
using ConsoleTools;
using Xunit;

namespace ConsoleMenuTests
{
  public class PreSelectionScenarioTest
  {
    [Fact]
    public void PreSelection_Simple()
    {
      var console = new TestConsole();

      var menu = new ConsoleMenu(args: new[] { "--menu-select=0", }, level: 0) { console = console }
      .Add("One", () => console.Write("Expected action"))
      .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
        m.ArgsPreselectedItemsKey = "--menu-select=";
      });
      menu.Show();

      Assert.Equal("Expected action", console.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void PreSelected_SubmenuItem()
    {
      var console = new TestConsole();

      var submenu = new ConsoleMenu(args: new[] { "--menu-select=0.1" }, level: 1) { console = console }
      .Add("One", () => { })
      .Add("Two", () => console.Write("Expected action"))
      .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });

      var menu = new ConsoleMenu(args: new[] { "--menu-select=0.1" }, level: 0) { console = console }
      .Add("One", submenu.Show)
      .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });
      menu.Show();

      Assert.Equal("Expected action", console.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void PreSelected_OpenSubmenu()
    {
      var console = new TestConsole();

      var submenu = new ConsoleMenu(args: new[] { "--menu-select=0" }, level: 1) { console = console }
      .Add("One1", () => console.Write("Should not be chosen"))
      .Add("Close1", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });

      var menu = new ConsoleMenu(args: new[] { "--menu-select=0" }, level: 0) { console = console }
      .Add("One0", submenu.Show)
      .Add("Close0", ConsoleMenu.Close)
      .Configure(m =>
      {
        ConfigHelper.BaseTestConfiguration(m, console);
      });

      console.AddUserInputWithActionBefore("2", () =>
      {
        Assert.Equal(@"Pick an option:
>> [0] One1
   [1] Close1
", console.ToString(), ignoreLineEndingDifferences: true);
      });

      console.AddUserInputWithActionBefore(ConsoleKey.Enter, () =>
      {
        submenu.CloseMenu();
        menu.CloseMenu();
      });

      menu.Show();
    }
  }
}
