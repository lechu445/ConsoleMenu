#pragma warning disable SA1401 // Fields should be private
using System;
using System.Collections.Generic;

namespace ConsoleTools;

/// <summary>
/// Menu configuration.
/// </summary>
public class MenuConfig
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MenuConfig"/> class.
  /// </summary>
  public MenuConfig()
  {
    this.WriteItemAction = item =>
    {
      if (this.EnableAlphabet)
      {
        AlphabetItemAction(item);
      }
      else
      {
        DefaultItemAction(item);
      }
    };
  }

  internal MenuConfig(MenuConfig config)
    : this()
  {
    this.ArgsPreselectedItemsKey = config.ArgsPreselectedItemsKey;
    this.ArgsPreselectedItemsValueSeparator = config.ArgsPreselectedItemsValueSeparator;
    this.ClearConsole = config.ClearConsole;
    this.EnableBreadcrumb = config.EnableBreadcrumb;
    this.EnableFilter = config.EnableFilter;
    this.EnableWriteTitle = config.EnableWriteTitle;
    this.FilterPrompt = config.FilterPrompt;
    this.ItemBackgroundColor = config.ItemBackgroundColor;
    this.ItemForegroundColor = config.ItemForegroundColor;
    this.SelectedItemBackgroundColor = config.SelectedItemBackgroundColor;
    this.SelectedItemForegroundColor = config.SelectedItemForegroundColor;
    this.Selector = config.Selector;
    this.Title = config.Title;
    this.WriteBreadcrumbAction = config.WriteBreadcrumbAction;
    this.WriteHeaderAction = config.WriteHeaderAction;
    this.WriteItemAction = config.WriteItemAction;
    this.WriteTitleAction = config.WriteTitleAction;
    this.EnableAlphabet = config.EnableAlphabet;
    this.DisableKeyboardNavigation = config.DisableKeyboardNavigation;
  }

  /// <summary>default: Console.ForegroundColor</summary>
  public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;

  /// <summary>default: Console.BackgroundColor</summary>
  public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;

  /// <summary>default: Console.BackgroundColor</summary>
  public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;

  /// <summary>default: Console.ForegroundColor</summary>
  public ConsoleColor ItemForegroundColor = Console.ForegroundColor;

  /// <summary>default: () => Console.WriteLine("Pick an option:")</summary>
  public Action WriteHeaderAction = () => Console.WriteLine("Pick an option:");

  /// <summary>default: (item) => Console.Write("[{0}] {1}", item.Index, item.Name)</summary>
  public Action<MenuItem> WriteItemAction;

  /// <summary>default: ">> "</summary>
  public string Selector = ">> ";

  /// <summary>default: "Filter: "</summary>
  public string FilterPrompt = "Filter: ";

  /// <summary>default: true</summary>
  public bool ClearConsole = true;

  /// <summary>default: true</summary>
  public bool EnableFilter = false;

  /// <summary>Console parameter that runs menu with pre-selection. default: "--menu-select="</summary>
  public string ArgsPreselectedItemsKey = "--menu-select=";

  /// <summary>default: '.'</summary>
  public char ArgsPreselectedItemsValueSeparator = '.';

  /// <summary>default: false</summary>
  public bool EnableWriteTitle = false;

  /// <summary>Menu title to write at top of the menu. default: "My menu"</summary>
  public string Title = "My menu";

  /// <summary>default: title => Console.WriteLine(title)</summary>
  public Action<string> WriteTitleAction = title => Console.WriteLine(title);

  /// <summary>default: false</summary>
  public bool EnableBreadcrumb = false;

  /// <summary>default: titles => Console.WriteLine(string.Join(" > ", titles))</summary>
  public Action<IReadOnlyList<string>> WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" > ", titles));

  /// <summary>
  /// Uses A..Z for the menu index to enable keyboard navigation for up to 36 menu items.
  /// If you have more than 36 menu items, it will switch back to using numbers for item 37 forward.
  /// default: false
  /// </summary>
  public bool EnableAlphabet = false;

  /// <summary>Disables keyboard navigation, forcing the use of the up and down arrows. default: false</summary>
  public bool DisableKeyboardNavigation = false;

  private static void DefaultItemAction(MenuItem item)
  {
    Console.Write("[{0}] {1}", item.Index, item.Name);
  }

  private static void AlphabetItemAction(MenuItem item)
  {
    char index;

    switch (item.Index)
    {
      case >= 36:
        // use item.Index (i.e. 37, 38, 39)
        Console.Write("[{0}] {1}", item.Index, item.Name);
        break;

      case >= 10:
        // use A..Z
        index = (char)(item.Index + 'A' - 10);
        Console.Write("[{0}] {1}", index, item.Name);
        break;

      default:
        // use 0..9
        index = (char)(item.Index + '0');
        Console.Write("[{0}] {1}", index, item.Name);
        break;
    }
  }
}
