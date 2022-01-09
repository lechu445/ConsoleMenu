using System;

namespace ConsoleTools;

public class MenuItem
{
  public string Name { get; set; }
  public Action Action { get; set; }
  public int Index { get; }

  internal MenuItem(string name, Action action, int index)
  {
    this.Name = name;
    this.Action = action;
    this.Index = index;
  }
}
