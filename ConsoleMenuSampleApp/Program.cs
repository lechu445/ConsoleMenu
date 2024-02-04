using System;
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
        EnableAlphabet = true,
        DisableKeyboardNavigation = false,
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
        .Add("Sub", subMenu.Show)
        .Add("Change me!", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
        .Add("Close", ConsoleMenu.Close)
        .Add("Action then Close", (thisMenu) => { SomeAction("Closing action..."); thisMenu.CloseMenu(); })
        .Add("Eight", () => SomeAction("Eight"))
        .Add("Nine", () => SomeAction("Nine"))
        .Add("Ten", () => SomeAction("Ten"))
        .Configure(commonConfig)
        .Configure(config =>
        {
          config.Title = "Main menu";
          config.EnableWriteTitle = true;
          config.EnableBreadcrumb = true;
        });

      for (int i = 0; i < 30; i++)
      {
        string word = NumberToWords(i + 11);
        menu.Add(word, () => SomeAction(word));
      }

      menu.Add("Exit", () => Environment.Exit(0));

      var token = new CancellationTokenSource(7000).Token;
      await menu.ShowAsync();
    }

    private static void SomeAction(string text)
    {
      Console.WriteLine(text);
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }

    private static async Task SomeAction2(CancellationToken token)
    {
      Console.WriteLine("start delay...");
      await Task.Delay(2000, token);
      Console.WriteLine("end delay");
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }

    private static string NumberToWords(int number)
    {
      if (number == 0)
        return "zero";

      if (number < 0)
        return "minus " + NumberToWords(Math.Abs(number));

      string words = "";

      if ((number / 1000000) > 0)
      {
        words += NumberToWords(number / 1000000) + " million ";
        number %= 1000000;
      }

      if ((number / 1000) > 0)
      {
        words += NumberToWords(number / 1000) + " thousand ";
        number %= 1000;
      }

      if ((number / 100) > 0)
      {
        words += NumberToWords(number / 100) + " hundred ";
        number %= 100;
      }

      if (number > 0)
      {
        if (words != "")
          words += "and ";

        var unitsMap = new[]
        {
          "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
          "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };
        var tensMap = new[]
        {
          "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };

        if (number < 20)
          words += unitsMap[number];
        else
        {
          words += tensMap[number / 10];
          if ((number % 10) > 0)
            words += "-" + unitsMap[number % 10];
        }
      }

      return words;
    }
  }
}
