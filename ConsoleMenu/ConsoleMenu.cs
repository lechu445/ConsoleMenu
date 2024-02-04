using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ConsoleMenuTests")]

namespace ConsoleTools;

/// <summary>
/// A simple, highly customizable, DOS-like console menu.
/// </summary>
public class ConsoleMenu : IEnumerable
{
  internal IConsole Console = new SystemConsole();
  private readonly ItemsCollection menuItems;
  private readonly CloseTrigger closeTrigger;
  private MenuConfig config = new MenuConfig();
  private ConsoleMenu? parent = null;
  private bool isShown = false;

  /// <summary>
  /// Initializes a new instance of the <see cref="ConsoleMenu"/> class.
  /// </summary>
  public ConsoleMenu()
  {
    this.menuItems = new ItemsCollection();
    this.closeTrigger = new CloseTrigger();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ConsoleMenu"/> class
  /// with possibility to pre-select items via console parameter.
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
      List<string> titles = new();
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
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  /// <exception cref="InvalidOperationException">Thrown if this method is called directly.</exception>
  public static Task Close(CancellationToken cancellationToken) => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

  /// <summary>
  /// Close the menu before or after a menu action was triggered.
  /// </summary>
  public void CloseMenu()
  {
    this.closeTrigger.SetOn();
  }

  /// <summary>
  /// Adds a menu action into this instance.
  /// </summary>
  /// <param name="name">Name of menu item.</param>
  /// <param name="action">Action to call when menu item is chosen.</param>
  /// <returns>This instance with added menu item.</returns>
  public ConsoleMenu Add(string name, Action action)
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

    this.menuItems.Add(name, (_) =>
    {
      action();
      return Task.CompletedTask;
    });
    return this;
  }

  /// <summary>
  /// Adds an asynchronous menu action into this instance.
  /// </summary>
  /// <param name="name">Name of menu item.</param>
  /// <param name="action">Action to call when menu item is chosen.</param>
  /// <returns>This instance with added menu item.</returns>
  public ConsoleMenu Add(string name, Func<CancellationToken, Task> action)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    if (action == null)
    {
      throw new ArgumentNullException(nameof(action));
    }

    if (action.Target is ConsoleMenu child && action == child.ShowAsync)
    {
      child.parent = this;
    }

    this.menuItems.Add(name, action);
    return this;
  }

  /// <summary>
  /// Adds a menu action into this instance.
  /// </summary>
  /// <param name="name">Name of menu item.</param>
  /// <param name="action">Action to call when menu item is chosen.</param>
  /// <returns>This instance with added menu item.</returns>
  public ConsoleMenu Add(string name, Action<ConsoleMenu> action)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    if (action is null)
    {
      throw new ArgumentNullException(nameof(action));
    }

    this.menuItems.Add(name, (_) =>
    {
      action(this);
      return Task.CompletedTask;
    });
    return this;
  }

  /// <summary>
  /// Adds an asynchronous menu action into this instance.
  /// </summary>
  /// <param name="name">Name of menu item.</param>
  /// <param name="action">Action to call when menu item is chosen.</param>
  /// <returns>This instance with added menu item.</returns>
  public ConsoleMenu Add(string name, Func<ConsoleMenu, CancellationToken, Task> action)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    if (action is null)
    {
      throw new ArgumentNullException(nameof(action));
    }

    this.menuItems.Add(name, (cancellationToken) => action(this, cancellationToken));
    return this;
  }

  /// <summary>
  /// Adds range of menu actions into this instance.
  /// </summary>
  /// <param name="menuItems">Menu items to add.</param>
  /// <returns>This instance with added menu items.</returns>
  public ConsoleMenu AddRange(IEnumerable<Tuple<string, Action>> menuItems)
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
  /// Adds range of asynchronous menu actions into this instance.
  /// </summary>
  /// <param name="menuItems">Menu items to add.</param>
  /// <returns>This instance with added menu items.</returns>
  public ConsoleMenu AddRange(IEnumerable<Tuple<string, Func<CancellationToken, Task>>> menuItems)
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
  /// Applies an configuration action on this instance.
  /// </summary>
  /// <param name="configure">Configuration action.</param>
  /// <returns>An configured instance.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="configure"/> is null.</exception>
  public ConsoleMenu Configure(Action<MenuConfig> configure)
  {
    if (configure is null)
    {
      throw new ArgumentNullException(nameof(configure));
    }

    configure.Invoke(this.config);
    return this;
  }

  /// <summary>
  /// Applies an configuration action on this instance.
  /// </summary>
  /// <param name="config">Configuration to apply.</param>
  /// <returns>An configured instance.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="config"/> is null.</exception>
  public ConsoleMenu Configure(MenuConfig config)
  {
    if (config is null)
    {
      throw new ArgumentNullException(nameof(config));
    }

    this.config = new MenuConfig(config);
    this.menuItems.EnableAlphabet = this.config.EnableAlphabet;
    return this;
  }

  /// <summary>
  /// Displays the menu in console.
  /// </summary>
  public void Show()
  {
    ShowAsync(CancellationToken.None).GetAwaiter().GetResult();
  }

  /// <summary>
  /// Displays the menu in console.
  /// </summary>
  public async Task ShowAsync(CancellationToken cancellationToken = default)
  {
    if (isShown)
    {
      this.menuItems.UnsetSelectedIndex();
    }

    isShown = true;

    await new ConsoleMenuDisplay(
        this.menuItems,
        this.Console,
        new List<string>(this.Titles),
        this.config,
        this.closeTrigger).ShowAsync(cancellationToken);
  }

  /// <summary>
  /// Returns an enumeration of the current menu items.
  /// See <see cref="Items"/>.
  /// </summary>
  /// <returns>An enumeration of the current menu items.</returns>
  public IEnumerator GetEnumerator() => this.Items.GetEnumerator();
}
