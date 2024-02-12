using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleTools;

namespace ConsoleMenuSampleApp
{
  public class Program
  {
    private static async Task Main(string[] args)
    {
      var commonConfig = new MenuConfig
      {
        Selector = "--> ",
        EnableFilter = true,
        EnableBreadcrumb = true,
        WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles)),
        OutputEncoding = Encoding.Unicode,
      };

      var subMenu1 = new ConsoleMenu(args, level: 2)
        .Add("One", () => SomeAction("One1"))
        .Add("Sub_Close", ConsoleMenu.Close)
        .Add("Sub_Exit", () => Environment.Exit(0))
        .Configure(commonConfig)
        .Configure(config =>
        {
          config.Title = "Submenu1";
        });

      var subMenu = new ConsoleMenu(args, level: 1)
        .Add("Sub_One", () => SomeAction("Sub_One"))
        .Add("Sub_Two", () => SomeAction("Sub_Two"))
        .Add("Sub_Three", () => SomeAction("Sub_Three"))
        .Add("Sub_Four", () => SomeAction("Sub_Four"))
        .Add("Sub_Five", async (cancellationToken) => await SomeAction2(cancellationToken))
        .Add("Sub_Close", ConsoleMenu.Close)
        .Add("Sub_Action then Close", (thisMenu) => { SomeAction("Closing action..."); thisMenu.CloseMenu(); })
        .Add("Sub_Exit", () => Environment.Exit(0))
        .Configure(commonConfig)
        .Configure(config =>
        {
          config.Title = "Submenu";
        });

      var menu = new ConsoleMenu(args, level: 0)
        .Add("One", () => SomeAction("One"))
        .Add("Two", () => SomeAction("Two"))
        .Add("Three", () => SomeAction("Three"))
        .Add("Sub \u00BB", subMenu.Show)
        .Add("Change me!", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
        .Add("Close", ConsoleMenu.Close)
        .Add("Action then Close", (thisMenu) => { SomeAction("Closing action..."); thisMenu.CloseMenu(); })
        .Add("Exit", () => Environment.Exit(0))
        .Configure(commonConfig)
        .Configure(config =>
        {
          config.Title = "Main menu";
          config.EnableWriteTitle = true;
          config.EnableBreadcrumb = true;
        });

      var token = new CancellationTokenSource(7000).Token;
      await menu.ShowAsync(token);
    }

    private static void SomeAction(string text)
    {
      Console.WriteLine(text);
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }

    private static async Task SomeAction2(CancellationToken token)
    {
      Console.WriteLine("start delay...");
      await Task.Delay(2000, token);
      Console.WriteLine("end delay");
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }
  }
}
