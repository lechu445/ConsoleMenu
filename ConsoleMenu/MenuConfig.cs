using System;
using System.Collections.Generic;

namespace ConsoleTools;

public class MenuConfig
{
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
  public Action<MenuItem> WriteItemAction = item => Console.Write("[{0}] {1}", item.Index, item.Name);

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
}
