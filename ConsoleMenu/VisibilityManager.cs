using System;
using System.Collections.Generic;

namespace ConsoleTools;

internal sealed class VisibilityManager
{
  private readonly bool[] visibility;

  public VisibilityManager(int size)
  {
    bool[] visibility = new bool[size];
    for (int i = 0; i < visibility.Length; i++)
    {
      visibility[i] = true; // true means visible
    }

    this.visibility = visibility;
  }

  public bool IsVisibleAt(int index)
  {
    return this.visibility[index];
  }

  public int IndexOfPreviousVisibleItem(int startIndex)
  {
    int idx = -1;
    if (startIndex - 1 >= 0)
    {
      idx = Array.LastIndexOf(this.visibility, true, startIndex - 1);
    }

    if (idx == -1)
    {
      idx = Array.LastIndexOf(this.visibility, true, this.visibility.Length - 1);
    }

    if (idx == -1)
    {
      idx = startIndex;
    }

    return idx;
  }

  public int IndexOfNextVisibleItem(int startIndex)
  {
    int idx = -1;
    if (startIndex + 1 < this.visibility.Length)
    {
      idx = Array.IndexOf(this.visibility, value: true, startIndex + 1);
    }

    if (idx == -1)
    {
      idx = Array.IndexOf(this.visibility, value: true, 0);
    }

    if (idx == -1)
    {
      idx = startIndex;
    }

    return idx;
  }

  public int IndexOfClosestVisibleItem(int startIndex)
  {
    // find closest next visible item
    var idx = Array.IndexOf(this.visibility, true, startIndex);
    if (idx == -1)
    {
      // find closest previous visible item
      idx = Array.LastIndexOf(this.visibility, true, startIndex);
    }

    if (idx == -1)
    {
      idx = 0;
    }

    return idx;
  }

  public void SetVisibleWithPredicate<T>(List<T> items, Predicate<T> isVisible)
  {
    for (int i = 0; i < this.visibility.Length; i++)
    {
      this.visibility[i] = isVisible(items[i]);
    }
  }
}
