using System;
using ConsoleTools;

namespace ConsoleMenuSampleApp
{
  public class Program
  {
    static void Main(string[] args)
    {
      var subMenu1 = new ConsoleMenu(args, level: 2)
        .Add("One", () => SomeAction("One1"))
          .Add("Sub_Close", ConsoleMenu.Close)
          .Add("Sub_Exit", () => Environment.Exit(0))
          .Configure(config =>
          {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Submenu1";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
          });

      var subMenu = new ConsoleMenu(args, level: 1)
      .Add("Sub", subMenu1.Show)
      .Add("Sub_Close", ConsoleMenu.Close)
      .Add("Sub_Exit", () => Environment.Exit(0))
      .Configure(config =>
      {
        config.Selector = "--> ";
        config.EnableFilter = true;
        config.Title = "Submenu";
        config.EnableBreadcrumb = true;
        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
      });

      var menu = new ConsoleMenu(args, level: 0)
        .Add("One", () => SomeAction("One"))
        .Add("Two", () => SomeAction("Two"))
        .Add("Three", () => SomeAction("Three"))
        .Add("Sub", subMenu.Show)
        .Add("Change me!", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
        .Add("Close", ConsoleMenu.Close)
        .Add("Action then Close", (thisMenu) => { SomeAction("Closing action..."); thisMenu.CloseMenu(); })
        .Add("Exit", () => Environment.Exit(0))
        .Configure(config =>
        {
          config.Selector = "--> ";
          config.EnableFilter = true;
          config.Title = "Main menu";
          config.EnableWriteTitle = true;
          config.EnableBreadcrumb = true;
        });

      menu.Show();
    }

    private static void SomeAction(string text)
    {
      Console.WriteLine(text);
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }
  }
}
