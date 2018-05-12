using ConsoleTools;
using System;

namespace ConsoleMenuSampleApp
{
  public class Program
  {
    static void Main(string[] args)
    {
      ConsoleMenu.Show(
        new MenuConfig { },
        ("One", () => Console.WriteLine("One")),
        ("Two", () => Console.WriteLine("Two")),
        ("Three", () => Console.WriteLine("Three")),
        ("Four", () => Console.WriteLine("Four")),
        ("Close", () => { }),
        ("Exit", () => Environment.Exit(0)));

      Console.ReadKey();
    }
  }
}
