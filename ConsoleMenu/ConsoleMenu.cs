using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConsoleMenuTests")]
namespace ConsoleTools;

public class ConsoleMenu
{
  internal IConsole console = new SystemConsole();
  private readonly MenuConfig _config = new MenuConfig();
  private readonly ItemsCollection menuItems;
  private readonly CloseTrigger closeTrigger;

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
  /// Menu items that can be modified.
  /// </summary>
  public IReadOnlyList<MenuItem> Items => menuItems.Items;

  /// <summary>
  /// Selected menu item that can be modified.
  /// </summary>
  public MenuItem CurrentItem
  {
    get => menuItems.CurrentItem;
    set => menuItems.CurrentItem = value;
  }

  private ConsoleMenu? Parent = null;

  private IReadOnlyList<string> Titles
  {
    get
    {
      ConsoleMenu? current = this;
      List<string> titles = new List<string>();
      while (current != null)
      {
        titles.Add(current._config.Title ?? "");
        current = current.Parent;
      }
      titles.Reverse();
      return titles;
    }
  }

  /// <summary>
  /// Close the menu before or after a menu action was triggered.
  /// </summary>
  public void CloseMenu()
  {
    this.closeTrigger.SetOn();
  }
  /// <summary>
  /// 
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
      child.Parent = this;
    }

    menuItems.Add(name, action);
    return this;
  }

  /// <summary>
  /// 
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

    menuItems.Add(name, () => action(this));
    return this;
  }

  /// <summary>
  /// 
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
      Add(item.Item1, item.Item2);
    }

    return this;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="configure"></param>
  /// <returns></returns>
  public ConsoleMenu Configure(Action<MenuConfig>? configure)
  {
    if (configure is null)
    {
      throw new ArgumentNullException(nameof(configure));
    }

    configure?.Invoke(_config);
    return this;
  }

  /// <summary>
  /// Don't run this method directly. Just pass a reference to this method.
  /// </summary>
  public static void Close() => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

  /// <summary>
  /// 
  /// </summary>
  public void Show()
  {
    new ConsoleMenuDisplay(
        this.menuItems,
        this.console,
        new List<string>(this.Titles),
        this._config,
        this.closeTrigger).Show();
  }
}
