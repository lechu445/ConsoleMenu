using System;

namespace ConsoleTools
{
  public static class ConsoleMenu
  {
    public static void Show(params (string name, Action action)[] menuItems)
    {
      Show(null, menuItems);
    }

    public static void Show(MenuConfig config = null, params (string name, Action action)[] menuItems)
    {
      config = config ?? new MenuConfig();
      ConsoleKeyInfo key;
      int curItem = 0;
      var currentForegroundColor = Console.ForegroundColor;
      var currentBackgroundColor = Console.BackgroundColor;
      bool breakIteration = false;
      do
      {
        if (config.ClearConsole)
        {
          Console.Clear();
        }
        config.WriteHeaderAction();

        for (int i = 0; i < menuItems.Length; i++)
        {
          if (curItem == i)
          {
            Console.BackgroundColor = config.SelectedItemBackgroundColor;
            Console.ForegroundColor = config.SelectedItemForegroundColor;
            Console.Write(config.Selector);
            config.WriteItemAction((menuItems[i].name, i));
            Console.WriteLine();
            Console.BackgroundColor = config.ItemBackgroundColor;
            Console.ForegroundColor = config.ItemForegroundColor;
          }
          else
          {
            Console.BackgroundColor = config.ItemBackgroundColor;
            Console.ForegroundColor = config.ItemForegroundColor;
            Console.Write(new string(' ', config.Selector.Length));
            config.WriteItemAction((menuItems[i].name, i));
            Console.WriteLine();
          }
        }

        if (breakIteration)
        {
          break;
        }

        readKey:
        key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.DownArrow)
        {
          curItem++;
          if (curItem > menuItems.Length - 1) curItem = 0;
        }
        else if (key.Key == ConsoleKey.UpArrow)
        {
          curItem--;
          if (curItem < 0) curItem = menuItems.Length - 1;
        }
        else if (key.KeyChar >= '0' && (key.KeyChar - '0') < menuItems.Length)
        {
          curItem = key.KeyChar - '0';
          breakIteration = true;
        }
        else if (key.Key != ConsoleKey.Enter)
        {
          goto readKey;
        }
      } while (key.Key != ConsoleKey.Enter);

      Console.ForegroundColor = currentForegroundColor;
      Console.BackgroundColor = currentBackgroundColor;
      menuItems[curItem].action();
    }
  }

  public class MenuConfig
  {
    public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public Action WriteHeaderAction = () => Console.WriteLine("Pick an option:");
    public Action<(string itemName, int itemIndex)> WriteItemAction = item => Console.Write("[{0}] {1}", item.itemIndex, item.itemName);
    public string Selector = ">> ";
    public bool ClearConsole = true;
  }
}
