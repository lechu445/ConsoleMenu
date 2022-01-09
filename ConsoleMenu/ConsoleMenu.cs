using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConsoleMenuTests")]

namespace ConsoleTools;

public class ConsoleMenu
{
  internal IConsole Console = new SystemConsole();
  private readonly MenuConfig config = new MenuConfig();
  private readonly ItemsCollection menuItems;
  private readonly CloseTrigger closeTrigger;
  private ConsoleMenu? parent = null;

  /// <summary>
  /// Creates ConsoleMenu instance.
  /// </summary>
  public ConsoleMenu()
  {
    this.menuItems = new ItemsCollection();
    this.closeTrigger = new CloseTrigger();
  }

  /// <summary>
  /// Creates ConsoleMenu instance with possibility to pre-select items via console parameter.
  /// </summary>
  /// <param name="args">args collection from Main.</param>
  /// <param name="level">Level of whole menu.</param>
  public ConsoleMenu(string[] args, int level)
  {
    if (args == null)
    {
      throw new ArgumentNullException(nameof(args));
    }

    if (level < 0)
    {
      throw new ArgumentException("Cannot be below 0", nameof(level));
    }

    this.menuItems = new ItemsCollection(args, level);
    this.closeTrigger = new CloseTrigger();
  }

  /// <summary>
  /// Gets menu items that can be modified.
  /// </summary>
  public IReadOnlyList<MenuItem> Items => this.menuItems.Items;

  /// <summary>
  /// Gets or sets selected menu item that can be modified.
  /// </summary>
  public MenuItem CurrentItem
  {
    get => this.menuItems.CurrentItem;
    set => this.menuItems.CurrentItem = value;
  }

  private IReadOnlyList<string> Titles
  {
    get
    {
      ConsoleMenu? current = this;
      List<string> titles = new List<string>();
      while (current != null)
      {
        titles.Add(current.config.Title ?? "");
        current = current.parent;
      }

      titles.Reverse();
      return titles;
    }
  }

  /// <summary>
  /// Don't run this method directly. Just pass a reference to this method.
  /// </summary>
  public static void Close() => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

  /// <summary>
  /// Close the menu before or after a menu action was triggered.
  /// </summary>
  public void CloseMenu()
  {
    this.closeTrigger.SetOn();
  }

  /// <summary>
  /// </summary>
  /// <param name="name"></param>
  /// <param name="action"></param>
  /// <returns></returns>
  public ConsoleMenu Add(string? name, Action? action)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    if (action == null)
    {
      throw new ArgumentNullException(nameof(action));
    }

    if (action.Target is ConsoleMenu child && action == child.Show)
    {
      child.parent = this;
    }

    this.menuItems.Add(name, action);
    return this;
  }

  /// <summary>
  /// </summary>
  /// <param name="name"></param>
  /// <param name="action"></param>
  /// <returns></returns>
  public ConsoleMenu Add(string? name, Action<ConsoleMenu>? action)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    if (action is null)
    {
      throw new ArgumentNullException(nameof(action));
    }

    this.menuItems.Add(name, () => action(this));
    return this;
  }

  /// <summary>
  /// </summary>
  /// <param name="menuItems"></param>
  /// <returns></returns>
  public ConsoleMenu AddRange(IEnumerable<Tuple<string, Action>>? menuItems)
  {
    if (menuItems is null)
    {
      throw new ArgumentNullException(nameof(menuItems));
    }

    foreach (var item in menuItems)
    {
      this.Add(item.Item1, item.Item2);
    }

    return this;
  }

  /// <summary>
  /// </summary>
  /// <param name="configure"></param>
  /// <returns></returns>
  public ConsoleMenu Configure(Action<MenuConfig>? configure)
  {
    if (configure is null)
    {
      throw new ArgumentNullException(nameof(configure));
    }

    configure?.Invoke(this.config);
    return this;
  }

  /// <summary>
  /// </summary>
  public void Show()
  {
    new ConsoleMenuDisplay(
        this.menuItems,
        this.Console,
        new List<string>(this.Titles),
        this.config,
        this.closeTrigger).Show();
  }
}
