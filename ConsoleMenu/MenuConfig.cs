#pragma warning disable SA1401 // Fields should be private
using System;
using System.Collections.Generic;
using System.Text;

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
  }

  internal MenuConfig(MenuConfig config)
  {
    this.ArgsPreselectedItemsKey = config.ArgsPreselectedItemsKey;
    this.ArgsPreselectedItemsValueSeparator = config.ArgsPreselectedItemsValueSeparator;
    this.ClearConsole = config.ClearConsole;
    this.EnableBreadcrumb = config.EnableBreadcrumb;
    this.EnableFilter = config.EnableFilter;
    this.EnableWriteTitle = config.EnableWriteTitle;
    this.FilterPrompt = config.FilterPrompt;
    this.InputEncoding = config.InputEncoding;
    this.ItemBackgroundColor = config.ItemBackgroundColor;
    this.ItemForegroundColor = config.ItemForegroundColor;
    this.OutputEncoding = config.OutputEncoding;
    this.SelectedItemBackgroundColor = config.SelectedItemBackgroundColor;
    this.SelectedItemForegroundColor = config.SelectedItemForegroundColor;
    this.Selector = config.Selector;
    this.Title = config.Title;
    this.WriteBreadcrumbAction = config.WriteBreadcrumbAction;
    this.WriteHeaderAction = config.WriteHeaderAction;
    this.WriteItemAction = config.WriteItemAction;
    this.WriteTitleAction = config.WriteTitleAction;
  }

  /// <summary>default: Console.ForegroundColor</summary>
  public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;

  /// <summary>default: Console.BackgroundColor</summary>
  public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;

  /// <summary>default: Console.BackgroundColor</summary>
  public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;

  /// <summary>default: Console.ForegroundColor</summary>
  public ConsoleColor ItemForegroundColor = Console.ForegroundColor;

  /// <summary>default: Console.OutputEncoding</summary>
  public Encoding InputEncoding = Console.InputEncoding;

  /// <summary>default: Console.OutputEncoding</summary>
  public Encoding OutputEncoding = Console.OutputEncoding;

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
