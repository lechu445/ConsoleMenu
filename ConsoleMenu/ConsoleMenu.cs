using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: InternalsVisibleTo("ConsoleMenuTests")]
namespace ConsoleTools
{
  public class ConsoleMenu
  {
    private readonly MenuConfig _config = new MenuConfig();
    private readonly List<MenuItem> _menuItems = new List<MenuItem>();
    private int? _selectedIndex;
    private string _selectedName;
    private bool close;
    private int currentItemIndex;
    internal IConsole console = new SystemConsole();

    /// <summary>
    /// Menu items that can be modified
    /// </summary>
    public IReadOnlyList<MenuItem> Items => _menuItems;

    /// <summary>
    /// Selected menu item that can be modified
    /// </summary>
    public MenuItem CurrentItem
    {
      get => _menuItems[currentItemIndex];
      set => _menuItems[currentItemIndex] = value;
    }

    /// <summary>
    /// Creates ConsoleMenu instance
    /// </summary>
    public ConsoleMenu() { }

    /// <summary>
    /// Creates ConsoleMenu instance with possibility to pre-select items via console parameter
    /// </summary>
    /// <param name="args">args collection from Main</param>
    /// <param name="level">Level of whole menu</param>
    public ConsoleMenu(string[] args, int level)
    {
      if(level < 0)
      {
        throw new ArgumentException("Cannot be below 0", nameof(level));
      }
      if(args == null)
      {
        throw new ArgumentNullException(nameof(args));
      }
      SetSeletedItems(args, level);
    }

    /// <summary>
    /// Close the menu before or after a menu action was triggered
    /// </summary>
    public void CloseMenu()
    {
      this.close = true;
    }

    private void SetSeletedItems(string[] args, int level)
    {
      var arg = Array.Find(args, a => a.StartsWith(_config.ArgsPreselectedItemsKey));
      SetSelectedItems(level, _config.ArgsPreselectedItemsKey, ref arg);
    }

    private void SetSelectedItems(int level, string paramKey, ref string arg)
    {
      if (arg == null)
      {
        return;
      }
      arg = arg.Replace(paramKey, string.Empty).Trim();
      var items = arg.SplitItems(_config.ArgsPreselectedItemsValueSeparator, '\'');
      if (level <= items.Count)
      {
        var item = items[level].Trim('\'');
        if (int.TryParse(item, out var selectedIndex))
        {
          _selectedIndex = selectedIndex;
          return;
        }
        _selectedName = item;
      }
    }

    public ConsoleMenu Add(string name, Action action)
    {
      _menuItems.Add(new MenuItem(name, action, index: _menuItems.Count));
      return this;
    }

    public ConsoleMenu Add(string name, Action<ConsoleMenu> action)
    {
      _menuItems.Add(new MenuItem(name, () => action(this), index: _menuItems.Count));
      return this;
    }

    public ConsoleMenu AddRange(IEnumerable<Tuple<string, Action>> menuItems)
    {
      foreach (var item in menuItems)
      {
        Add(item.Item1, item.Item2);
      }
      return this;
    }

    public ConsoleMenu Configure(Action<MenuConfig> configure)
    {
      configure?.Invoke(_config);
      return this;
    }

    /// <summary>
    /// Don't run this method directly. Just pass a reference to this method.
    /// </summary>
    public static void Close() => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

    public void Show()
    {
      var selectedItem = GetSeletedItem();
      if(selectedItem != null)
      {
        selectedItem.Action.Invoke();
        return;
      }
      ConsoleKeyInfo key;
      bool[] visibility = CreateVisibility(); //true means visible
      this.currentItemIndex = 0;
      var currentForegroundColor = this.console.ForegroundColor;
      var currentBackgroundColor = this.console.BackgroundColor;
      bool breakIteration = false;
      var filter = new StringBuilder();

      while (true)
      {
        do
        {
          redraw:
          if (_config.ClearConsole)
          {
            this.console.Clear();
          }
          _config.WriteHeaderAction();

          int i = 0;
          foreach (var menuItem in _menuItems)
          {
            if (_config.EnableFilter && !visibility[i])
            {
              currentItemIndex = SetAnotherCurItem(visibility, currentItemIndex, out var shouldRedraw);
              if (shouldRedraw)
              {
                goto redraw;
              }
              i++;
              continue;
            }
            if (currentItemIndex == i)
            {
              this.console.BackgroundColor = _config.SelectedItemBackgroundColor;
              this.console.ForegroundColor = _config.SelectedItemForegroundColor;
              this.console.Write(_config.Selector);
              _config.WriteItemAction(menuItem);
              this.console.WriteLine();
              this.console.BackgroundColor = _config.ItemBackgroundColor;
              this.console.ForegroundColor = _config.ItemForegroundColor;
            }
            else
            {
              this.console.BackgroundColor = _config.ItemBackgroundColor;
              this.console.ForegroundColor = _config.ItemForegroundColor;
              this.console.Write(new string(' ', _config.Selector.Length));
              _config.WriteItemAction(menuItem);
              this.console.WriteLine();
            }
            i++;
          }

          if (breakIteration)
          {
            breakIteration = false;
            break;
          }

          if (_config.EnableFilter)
          {
            this.console.Write(_config.FilterPrompt + filter);
          }

          readKey:
          key = this.console.ReadKey(true);

          if (key.Key == ConsoleKey.DownArrow)
          {
            currentItemIndex = IndexOfNextVisibleItem(currentItemIndex, visibility);
          }
          else if (key.Key == ConsoleKey.UpArrow)
          {
            currentItemIndex = IndexOfPreviousVisibleItem(currentItemIndex, visibility);
          }
          else if (key.KeyChar >= '0' && (key.KeyChar - '0') < _menuItems.Count)
          {
            currentItemIndex = key.KeyChar - '0';
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
              }
              else if(!char.IsControl(key.KeyChar))
              {
                filter.Append(key.KeyChar);
              }
              UpdateVisibility(_menuItems, visibility,
                (item) => item.Name.Contains(filter.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            else
            {
              goto readKey;
            }
          }
        } while (key.Key != ConsoleKey.Enter);

        this.console.WriteLine();
        this.console.ForegroundColor = currentForegroundColor;
        this.console.BackgroundColor = currentBackgroundColor;
        var action = _menuItems[currentItemIndex].Action;
        if (action == Close)
        {
          return;
        }
        else
        {
          action?.Invoke();
          if (this.close)
          {
            this.close = false;
            return;
          }
        }
      }
    }

    private MenuItem GetSeletedItem()
    {
      if(_selectedIndex.HasValue && _selectedIndex.Value < _menuItems.Count)
      {
        return _menuItems[_selectedIndex.Value];
      }
      if(_selectedName != null)
      {
        return _menuItems.Find(item => item.Name == _selectedName);
      }
      return null;
    }

    private int IndexOfNextVisibleItem(int curItem, bool[] visibility)
    {
      int idx = -1;
      if(curItem + 1 < visibility.Length)
      {
        idx = Array.IndexOf(visibility, value: true, startIndex: curItem + 1);
      }
      if (idx == -1)
      {
        idx = Array.IndexOf(visibility, value: true, startIndex: 0);
      }
      if(idx == -1)
      {
        idx = curItem;
      }
      return idx;
    }

    private int IndexOfPreviousVisibleItem(int curItem, bool[] visibility)
    {
      int idx = -1;
      if(curItem - 1 >= 0)
      {
        idx = Array.LastIndexOf(visibility, value: true, startIndex: curItem - 1);
      }
      if (idx == -1)
      {
        idx = Array.LastIndexOf(visibility, value: true, startIndex: visibility.Length - 1);
      }
      if (idx == -1)
      {
        idx = curItem;
      }
      return idx;
    }

    private static int SetAnotherCurItem(bool[] visibility, int curItem, out bool shouldRedraw)
    {
      shouldRedraw = false;
      var foundIdx = Array.IndexOf(visibility, true, curItem);
      if (foundIdx != -1)
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
  }
}
