using ConsoleTools;

namespace ConsoleMenuTests.TestHelpers
{
  internal static class ConfigHelper
  {
    public static void BaseTestConfiguration(MenuConfig m, TestConsole console)
    {
      m.SelectedItemBackgroundColor = console.ForegroundColor;
      m.SelectedItemForegroundColor = console.BackgroundColor;
      m.ItemBackgroundColor = console.BackgroundColor;
      m.ItemForegroundColor = console.ForegroundColor;
      m.WriteHeaderAction = () => console.WriteLine("Pick an option:");
      m.WriteItemAction = item => console.Write("[{0}] {1}", item.Index, item.Name);
      m.WriteTitleAction = title => console.WriteLine(title);
    }
  }
}
