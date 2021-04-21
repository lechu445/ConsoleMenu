using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTools;

internal sealed class ConsoleMenuDisplay
{
  private readonly IConsole console;
  private readonly ItemsCollection menuItems;
  private readonly List<string> titles;
  private readonly MenuConfig _config;
  private readonly VisibilityManager visibility;
  private readonly CloseTrigger closeTrigger;
  private readonly string noSelectorLine;

  public ConsoleMenuDisplay(
      ItemsCollection menuItems,
      IConsole console,
      List<string> titles,
      MenuConfig config,
      CloseTrigger closeTrigger)
  {
    this.menuItems = menuItems;
    this.console = console;
    this.titles = titles;
    this._config = config;
    this.visibility = new VisibilityManager(menuItems.Items.Count);
    this.closeTrigger = closeTrigger;
    this.noSelectorLine = new string(' ', _config.Selector.Length);
  }

  public void Show()
  {
    var selectedItem = menuItems.GetSeletedItem();
    if (selectedItem != null)
    {
      selectedItem.Action.Invoke();
      return;
    }

    ConsoleKeyInfo key;
    menuItems.ResetCurrentIndex();
    var currentForegroundColor = this.console.ForegroundColor;
    var currentBackgroundColor = this.console.BackgroundColor;
    bool breakIteration = false;
    var filter = new StringBuilder();

    while (true)
    {
      do
      {
        if (_config.ClearConsole)
        {
          this.console.Clear();
        }

        if (_config.EnableBreadcrumb)
        {
          _config.WriteBreadcrumbAction(this.titles);
        }

        if (_config.EnableWriteTitle)
        {
          _config.WriteTitleAction(_config.Title);
        }

        _config.WriteHeaderAction();

        foreach (var menuItem in menuItems.Items)
        {
          if (_config.EnableFilter && !visibility.IsVisibleAt(menuItem.Index))
          {
            menuItems.SelectClosestVisibleItem(visibility);
          }
          else
          {
            WriteLineWithItem(menuItem);
          }
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
          menuItems.SelectNextVisibleItem(visibility);
        }
        else if (key.Key == ConsoleKey.UpArrow)
        {
          menuItems.SelectPreviousVisibleItem(visibility);
        }
        else if (menuItems.CanSelect(key.KeyChar))
        {
          menuItems.Select(key.KeyChar);
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
            else if (!char.IsControl(key.KeyChar))
            {
              filter.Append(key.KeyChar);
            }

            var filterString = filter.ToString();

            visibility.SetVisibleWithPredicate(menuItems.Items, (item) => item.Name.Contains(filterString, StringComparison.OrdinalIgnoreCase));
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
      var action = menuItems.CurrentItem.Action;
      if (action == ConsoleMenu.Close)
      {
        return;
      }
      else
      {
        action();
        if (this.closeTrigger.IsOn())
        {
          this.closeTrigger.SetOff();
          return;
        }
      }
    }
  }

  private void WriteLineWithItem(MenuItem menuItem)
  {
    if (menuItems.IsSelected(menuItem))
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
      this.console.Write(this.noSelectorLine);
      _config.WriteItemAction(menuItem);
      this.console.WriteLine();
    }
  }
}
