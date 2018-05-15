using System;
using System.Collections.Generic;

namespace ConsoleTools
{
  public class ConsoleMenu
  {
    private readonly MenuConfig Config = new MenuConfig();
    private readonly List<Tuple<string, Action>> _menuItems = new List<Tuple<string, Action>>();

    public ConsoleMenu Add(string name, Action action)
    {
      _menuItems.Add(new Tuple<string, Action>(name, action));
      return this;
    }

    public ConsoleMenu AddRange(IEnumerable<Tuple<string, Action>> menuItems)
    {
      _menuItems.AddRange(menuItems);
      return this;
    }

    public ConsoleMenu Configure(Action<MenuConfig> configure)
    {
      configure?.Invoke(this.Config);
      return this;
    }

    public static void Close() => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

    public void Show()
    {
      ConsoleKeyInfo key;
      var menuItems = _menuItems.ToArray();
      int curItem = 0;
      var currentForegroundColor = Console.ForegroundColor;
      var currentBackgroundColor = Console.BackgroundColor;
      bool breakIteration = false;
      while (true)
      {
        do
        {
          if (Config.ClearConsole)
          {
            Console.Clear();
          }
          Config.WriteHeaderAction();

          for (int i = 0; i < menuItems.Length; i++)
          {
            var itemName = menuItems[i].Item1;
            if (curItem == i)
            {
              Console.BackgroundColor = Config.SelectedItemBackgroundColor;
              Console.ForegroundColor = Config.SelectedItemForegroundColor;
              Console.Write(Config.Selector);
              Config.WriteItemAction(new MenuItem { Name = itemName, Index = i });
              Console.WriteLine();
              Console.BackgroundColor = Config.ItemBackgroundColor;
              Console.ForegroundColor = Config.ItemForegroundColor;
            }
            else
            {
              Console.BackgroundColor = Config.ItemBackgroundColor;
              Console.ForegroundColor = Config.ItemForegroundColor;
              Console.Write(new string(' ', Config.Selector.Length));
              Config.WriteItemAction(new MenuItem { Name = itemName, Index = i });
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
        var action = menuItems[curItem].Item2;
        if (action == Close)
        {
          return;
        }
        else
        {
          action?.Invoke();
        }
      }
    }
  }

  public class MenuConfig
  {
    public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public Action WriteHeaderAction = () => Console.WriteLine("Pick an option:");
    public Action<MenuItem> WriteItemAction = item => Console.Write("[{0}] {1}", item.Index, item.Name);
    public string Selector = ">> ";
    public bool ClearConsole = true;
  }

  public struct MenuItem { public string Name; public int Index; };
}
