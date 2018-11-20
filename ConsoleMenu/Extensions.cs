using System;
using System.Collections.Generic;

namespace ConsoleTools
{
  internal static class Extensions
  {
    public static List<string> SplitItems(this string input, char separator, char itemQuote)
    {
      var result = new List<string>();
      var isInQuote = false;
      var start = 0;
      for (int i = 0; i < input.Length; i++)
      {
        var ch = input[i];
        if (ch == itemQuote)
        {
          isInQuote = !isInQuote;
        }
        if (!isInQuote && ch == separator)
        {
          result.Add(input.Substring(start, i - start));
          start = i + 1;
        }
      }
      if (start < input.Length)
      {
        result.Add(input.Substring(start, input.Length - start));
      }
      return result;
    }
  }
}
