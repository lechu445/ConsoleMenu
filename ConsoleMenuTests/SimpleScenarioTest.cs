using ConsoleTools;
using System;
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

      var menu = new ConsoleMenu() { console = console }
      .Add("One", () => { })
      .Add("Two", () => { })
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        m.SelectedItemBackgroundColor = console.ForegroundColor;
        m.SelectedItemForegroundColor = console.BackgroundColor;
        m.ItemBackgroundColor = console.BackgroundColor;
        m.ItemForegroundColor = console.ForegroundColor;
        m.WriteHeaderAction = () => console.WriteLine("Pick an option:");
        m.WriteItemAction = item => console.Write("[{0}] {1}", item.Index, item.Name);
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
    public void Breadcrumbs()
    {
      var console = new TestConsole();
      console.AddUserInputWithActionBefore("1", () => Assert.Equal(@"First menu
Pick an option:
>> [0] One
   [1] Two
   [2] Close
   [3] Exit
", console.ToString(), ignoreLineEndingDifferences: true));

      console.AddUserInputWithActionBefore("0", () => Assert.Equal(@"First menu > Second menu
Pick an option:
>> [0] Close
", console.ToString(), ignoreLineEndingDifferences: true));

      console.AddUserInputWithActionBefore("2", () => Assert.Equal(@"First menu
Pick an option:
   [0] One
>> [1] Two
   [2] Close
   [3] Exit
", console.ToString(), ignoreLineEndingDifferences: true));

      var submenu = new ConsoleMenu { console = console }
        .Add("Close", ConsoleMenu.Close)
      .Configure(m =>
      {
        m.WriteHeaderAction = () => console.WriteLine("Pick an option:");
        m.WriteItemAction = item => console.Write("[{0}] {1}", item.Index, item.Name);
        m.EnableBreadcrumb = true;
        m.WriteBreadcrumbAction = titles => console.WriteLine(string.Join(" > ", titles));
        m.Title = "Second menu";
      });

      var menu = new ConsoleMenu() { console = console }
      .Add("One", () => { })
      .Add("Two", submenu.Show)
      .Add("Close", ConsoleMenu.Close)
      .Add("Exit", () => Environment.Exit(0))
      .Configure(m =>
      {
        m.WriteHeaderAction = () => console.WriteLine("Pick an option:");
        m.WriteItemAction = item => console.Write("[{0}] {1}", item.Index, item.Name);
        m.EnableBreadcrumb = true;
        m.WriteBreadcrumbAction = titles => console.WriteLine(string.Join(" > ", titles));
        m.Title = "First menu";
      });
      menu.Show();

      Assert.Equal(@"First menu
Pick an option:
   [0] One
   [1] Two
>> [2] Close
   [3] Exit

", console.ToString(), ignoreLineEndingDifferences: true);
    }
  }
}
