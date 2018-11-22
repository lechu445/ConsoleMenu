# ConsoleMenu
A simple, highly customizable, DOS-like console menu

![img](https://raw.githubusercontent.com/lechu445/ConsoleMenu/master/preview.gif)

Nuget package: https://www.nuget.org/packages/ConsoleMenu-simple

## Usage
```csharp
      var subMenu = new ConsoleMenu(args, level: 1)
        .Add("Sub_One", () => SomeAction("Sub_One"))
        .Add("Sub_Two", () => SomeAction("Sub_Two"))
        .Add("Sub_Three", () => SomeAction("Sub_Three"))
        .Add("Sub_Four", () => SomeAction("Sub_Four"))
        .Add("Sub_Close", ConsoleMenu.Close)
        .Add("Sub_Exit", () => Environment.Exit(0));
        
      var menu = new ConsoleMenu(args, level: 0)
        .Add("One", () => SomeAction("One"))
        .Add("Two", () => SomeAction("Two"))
        .Add("Three", () => SomeAction("Three"))
        .Add("Sub", subMenu.Show)
        .Add("Close", ConsoleMenu.Close)
        .Add("Exit", () => Environment.Exit(0))
        .Configure(config => { config.Selector = "--> "; });

      menu.Show();
```

### Running app from console with pre-selected menu items 
To do this, use `public ConsoleMenu(string[] args, int level)` constructor during initialization.
Use double quotes for item names and digits for item numbers. Here are some examples:
```csharp
--menu-select=0.1                      //run first at level 0 and second at level 1
--menu-select="Sub.Sub_One.'Close...'" //run "Sub" at level 0 and "Sub_One" at level 1, and "Close..." at level 2
--menu-select="Sub.2"                  //run item "Sub" at level 0, and then run third item at level 1
```

### Configuration
You can also define configuration via .Configure() method. The default config looks like:
```csharp
  public class MenuConfig
  {
    public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public Action WriteHeaderAction = () => Console.WriteLine("Pick an option:");
    public Action<MenuItem> WriteItemAction = item => Console.Write("[{0}] {1}", item.Index, item.Name);
    public string Selector = ">> ";
    public string FilterPrompt = "Filter: ";
    public bool ClearConsole = true;
    public bool EnableFilter = false;
    public string ArgsPreselectedItemsKey = "--menu-select=";
    public char ArgsPreselectedItemsValueSeparator = '.';
  }
```
Example:
```csharp
      new ConsoleMenu()
        .Add("One", () => SomeAction("One"))
        .Add("Two", () => SomeAction("Two"))
        .Add("Close", ConsoleMenu.Close)
        .Configure(config => { config.Selector = "--> "; })
        .Show();
```
## Requirements
Framework compatible with .NET Standard 1.3 (.NET Core 1.0, .NET Framework 4.6, Mono 4.6) or higher.
