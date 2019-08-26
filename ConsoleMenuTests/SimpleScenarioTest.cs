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
      const string input = @"2
";
      var console = new TestConsole(input);

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

", console.ToString());
    }
  }
}
