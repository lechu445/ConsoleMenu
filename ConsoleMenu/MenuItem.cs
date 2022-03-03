using System;
using System.Diagnostics;

namespace ConsoleTools;

/// <summary>
/// Menu item.
/// </summary>
public sealed class MenuItem
{
  internal MenuItem(string name, Action action, int index)
  {
    Debug.Assert(index >= 0);

    this.Name = name ?? throw new ArgumentNullException(nameof(name));
    this.Action = action ?? throw new ArgumentNullException(nameof(action));
    this.Index = index;
  }

  /// <summary>
  /// Gets or sets name of the menu item that will be displayed.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Gets or sets an action of the menu item that will be called when the item is called.
  /// </summary>
  public Action Action { get; set; }

  /// <summary>
  /// Gets an index of the menu item.
  /// </summary>
  public int Index { get; }
}
