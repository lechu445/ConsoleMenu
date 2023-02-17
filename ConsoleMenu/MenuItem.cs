using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTools;

/// <summary>
/// Menu item.
/// </summary>
public sealed class MenuItem
{
  internal MenuItem(string name, Func<CancellationToken, Task> action, int index)
  {
    Debug.Assert(index >= 0);

    this.Name = name ?? throw new ArgumentNullException(nameof(name));
    this.AsyncAction = action ?? throw new ArgumentNullException(nameof(action));
    this.Index = index;
  }

  /// <summary>
  /// Gets or sets name of the menu item that will be displayed.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Gets or sets an action of the menu item that will be called when the item is called.
  /// If you get asynchronous action, it will be converted to synchronous, so better use <see cref="AsyncAction"/> getter.
  /// </summary>
  public Action Action
  {
    get => () => this.AsyncAction(CancellationToken.None).GetAwaiter().GetResult();
    set => this.AsyncAction = (_) =>
    {
      value();
      return Task.CompletedTask;
    };
  }

  /// <summary>
  /// Gets or sets an action of the menu item that will be called when the item is called.
  /// </summary>
  public Func<CancellationToken, Task> AsyncAction { get; set; }

  /// <summary>
  /// Gets an index of the menu item.
  /// </summary>
  public int Index { get; }
}
