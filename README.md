# ConsoleMenu
A simple, highly customizable, DOS-like console menu

![img](https://raw.githubusercontent.com/lechu445/ConsoleMenu/master/preview.gif)

## Usage
```csharp
ConsoleMenu.Show(
        new MenuConfig(),
        ("First menu item", () => { /* action of chosen first item */ }),
        ("Second menu item", () => { /* action of chosen second item */ }),
        ("Close menu", () => { }),
        ("Exit application", () => Environment.Exit(0)));
```
### Configuration
You can also define configuration via MenuConfig object. The default config looks like:
```csharp
  public class MenuConfig
  {
    public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public Action WriteHeaderAction = () => Console.WriteLine("Pick an option:");
    public Action<(string itemName, int itemIndex)> WriteItemAction = item => Console.Write("[{0}] {1}", item.itemIndex, item.itemName);
    public string Selector = ">> ";
    public bool ClearConsole = true;
  }
```
Example:
```csharp
ConsoleMenu.Show(
        new MenuConfig { Selector = "-->" },
        ("First menu item", () => { /* action of chosen first item */ }),
        ("Second menu item", () => { /* action of chosen second item */ }),
        ("Exit application", () => Environment.Exit(0)));
```
## Requirements
Used framework compatible with .NET Standard 2.0 (.NET Core 2.0, .NET Framework 4.6.1, Mono 5.4) or higher.
