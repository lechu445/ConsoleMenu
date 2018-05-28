using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTools
{
  public class ConsoleMenu
  {
    private readonly MenuConfig _config = new MenuConfig();
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
      configure?.Invoke(_config);
      return this;
    }

    public static void Close() => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

    public void Show()
    {
      ConsoleKeyInfo key;
      bool[] visibility = CreateVisibility();
      int curItem = 0;
      var currentForegroundColor = Console.ForegroundColor;
      var currentBackgroundColor = Console.BackgroundColor;
      bool breakIteration = false;
      var filter = new StringBuilder();

      while (true)
      {
        do
        {
          redraw:
          if (_config.ClearConsole)
          {
            Console.Clear();
          }
          _config.WriteHeaderAction();

          int i = 0;
          foreach (var menuItem in _menuItems)
          {
            if(_config.EnableFilter && !visibility[i])
            {
              curItem = SetAnotherCurItem(visibility, curItem, out var shouldRedraw);
              if (shouldRedraw)
              {
                goto redraw;
              }
              i++;
              continue;
            }
            var itemName = menuItem.Item1;
            if (curItem == i)
            {
              Console.BackgroundColor = _config.SelectedItemBackgroundColor;
              Console.ForegroundColor = _config.SelectedItemForegroundColor;
              Console.Write(_config.Selector);
              _config.WriteItemAction(new MenuItem { Name = itemName, Index = i });
              Console.WriteLine();
              Console.BackgroundColor = _config.ItemBackgroundColor;
              Console.ForegroundColor = _config.ItemForegroundColor;
            }
            else
            {
              Console.BackgroundColor = _config.ItemBackgroundColor;
              Console.ForegroundColor = _config.ItemForegroundColor;
              Console.Write(new string(' ', _config.Selector.Length));
              _config.WriteItemAction(new MenuItem { Name = itemName, Index = i });
              Console.WriteLine();
            }
            i++;
          }

          if (breakIteration)
          {
            break;
          }

          if(_config.EnableFilter)
          {
            Console.Write(_config.FilterPrompt + filter);
          }

          readKey:
          key = Console.ReadKey(true);

          if (key.Key == ConsoleKey.DownArrow)
          {
            curItem++;
            if (curItem > _menuItems.Count - 1) curItem = 0;
          }
          else if (key.Key == ConsoleKey.UpArrow)
          {
            curItem--;
            if (curItem < 0) curItem = _menuItems.Count - 1;
          }
          else if (key.KeyChar >= '0' && (key.KeyChar - '0') < _menuItems.Count)
          {
            curItem = key.KeyChar - '0';
            breakIteration = true;
          }
          else if (key.Key != ConsoleKey.Enter)
          {
            if (_config.EnableFilter)
            {
              if (key.Key == ConsoleKey.Backspace)
              {
                if (filter.Length > 0)
                {
                  filter.Length--;
                }
                Console.Write("\b \b");
              }
              else
              {
                filter.Append(key.KeyChar);
                Console.Write(key.KeyChar);
              }
              UpdateVisibility(_menuItems, visibility, (item) => Contains(item.Item1, filter.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            else
            {
              goto readKey;
            }
          }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        Console.ForegroundColor = currentForegroundColor;
        Console.BackgroundColor = currentBackgroundColor;
        var action = _menuItems[curItem].Item2;
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

    private static int SetAnotherCurItem(bool[] visibility, int curItem, out bool shouldRedraw)
    {
      shouldRedraw = false;
      var foundIdx = Array.IndexOf(visibility, true, curItem);
      if(foundIdx != -1)
      {
        return foundIdx;
      }
      foundIdx = Array.LastIndexOf(visibility, true, curItem);
      if (foundIdx != -1)
      {
        shouldRedraw = true;
        return foundIdx;
      }
      return foundIdx == -1 ? 0 : foundIdx;
    }

    private bool[] CreateVisibility()
    {
      bool[] visibility = new bool[_menuItems.Count];
      for (int i = 0; i < visibility.Length; i++)
      {
        visibility[i] = true;
      }
      return visibility;
    }

    private static void UpdateVisibility<T>(List<T> items, bool[] visibility, Predicate<T> matchFilter)
    {
      for (int i = 0; i < visibility.Length; i++)
      {
        visibility[i] = matchFilter(items[i]);
      }
    }

    public static bool Contains(string source, string toCheck, StringComparison comp)
    {
      return source?.IndexOf(toCheck, comp) >= 0;
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
    public string FilterPrompt = "Filter: ";
    public bool ClearConsole = true;
    public bool EnableFilter = false;
  }

  public struct MenuItem { public string Name; public int Index; };
}
