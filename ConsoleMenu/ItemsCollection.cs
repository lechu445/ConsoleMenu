using System;
using System.Collections.Generic;

namespace ConsoleTools;

internal sealed class ItemsCollection
{
  private readonly List<MenuItem> _menuItems = new List<MenuItem>();
  private readonly MenuConfig _config = new MenuConfig();
  private int? _selectedIndex;
  private string? _selectedName;
  private int currentItemIndex;

  public ItemsCollection()
  {
  }

  public ItemsCollection(string[] args, int level)
  {
    SetSelectedItems(args, level);
  }

  public List<MenuItem> Items => _menuItems;

  public MenuItem CurrentItem
  {
    get => _menuItems[currentItemIndex];
    set => _menuItems[currentItemIndex] = value;
  }

  public void Add(string name, Action action)
  {
    _menuItems.Add(new MenuItem(name, action, _menuItems.Count));
  }

  public void ResetCurrentIndex()
  {
    this._selectedIndex = 0;
  }

  public void SetSelectedItems(string[] args, int level)
  {
    var arg = Array.Find(args, a => a.StartsWith(_config.ArgsPreselectedItemsKey));
    SetSelectedItems(level, _config.ArgsPreselectedItemsKey, ref arg);
  }

  private void SetSelectedItems(int level, string paramKey, ref string? arg)
  {
    if (arg == null)
    {
      return;
    }

    arg = arg.Replace(paramKey, string.Empty).Trim();
    var items = arg.SplitItems(_config.ArgsPreselectedItemsValueSeparator, '\'');
    if (level < items.Count)
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

  public MenuItem? GetSeletedItem()
  {
    if (_selectedIndex.HasValue && _selectedIndex.Value < _menuItems.Count)
    {
      return _menuItems[_selectedIndex.Value];
    }

    if (_selectedName != null)
    {
      return _menuItems.Find(item => item.Name == _selectedName);
    }

    return null;
  }

  public void SelectClosestVisibleItem(VisibilityManager visibility)
  {
    currentItemIndex = visibility.IndexOfClosestVisibleItem(currentItemIndex);
  }

  public void SelectNextVisibleItem(VisibilityManager visibility)
  {
    this.currentItemIndex = visibility.IndexOfNextVisibleItem(this.currentItemIndex);
  }

  public void SelectPreviousVisibleItem(VisibilityManager visibility)
  {
    this.currentItemIndex = visibility.IndexOfPreviousVisibleItem(this.currentItemIndex);
  }

  public bool CanSelect(char ch)
  {
    return ch >= '0' && (ch - '0') < _menuItems.Count; // is in range 0.._menuItems.Count
  }

  public void Select(char ch)
  {
    currentItemIndex = ch - '0';
  }

  internal bool IsSelected(MenuItem menuItem)
  {
    return currentItemIndex == menuItem.Index;
  }
}
