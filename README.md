# Lakerfield.ConsoleMenu
A simple fluent console menu build in C# with support for asynchronously executing tasks

## Packages
Published as a [NuGet package](https://www.nuget.org/packages/Lakerfield.ConsoleMenu)

## Usage examples

### Minimal menu
```csharp
await ConsoleMenu.RunMainMenuAndWaitForCompletion(
  "Lakerfield minimal sample menu", 
  menu =>
  {
    // Add option
    sub.Add(ConsoleKey.H, "Say Hi", async () =>
    {
      await Task.Delay(100);
      Console.WriteLine("Hi");
    });

    // Nothing
    sub.Add(ConsoleKey.N, "Nothing", () => Task.CompletedTask);
  }, 
  onlyExitWhenPressingQuit: false);
  ```

### A menu with 3 items where one is executed asynchronously
```csharp
Console.WriteLine("Hello World! Check out this menu...");
await ConsoleMenu.RunMainMenuAndWaitForCompletion(
  "Lakerfield Sample menu", 
  menu =>
  {
    // Add add class implementing IConsoleTask
    menu.Add<ListFiles>();

    // Add instance of a IConsoleTask
    menu.Add(new WaitSeconds(2));

    // Add instance of a IConsoleTask and execute it asynchronously
    menu.Add(new WaitSeconds(2), waitForCompletion:false, overrideTitle: "async wait 2 seconds");
  });


public class ListFiles : IConsoleTask
{
  public string Title { get; }
  public Task Execute()
  {
    foreach (var file in new DirectoryInfo(Environment.CurrentDirectory).GetFiles())
      Console.WriteLine($"- {file.Name}");
    return Task.CompletedTask;
  }
}
```

### Sub menus
```csharp
Console.WriteLine("Hello World! Check out this submenu...");
await ConsoleMenu.RunMainMenuAndWaitForCompletion(
  "Lakerfield submenu sample menu", 
  menu =>
  {
    // Add submenu
    menu.AddSubMenu("My sub menu",
      sub =>
      {
        // Override title
        sub.Add<ListFiles>(overrideTitle: "Show me the files");

        // Override key
        sub.Add<ListFiles>(overrideKey: ConsoleKey.Z);

        // Add 
        sub.Add(ConsoleKey.H, "Say Hi", async () =>
        {
          Console.WriteLine("Hi");
          await Task.Delay(100);
          Console.WriteLine("   it");
          await Task.Delay(100);
          Console.WriteLine("      is");
          await Task.Delay(100);
          Console.WriteLine("         me");
        });

        // Nothing
        sub.Add(ConsoleKey.N, "Nothing", () => Task.CompletedTask);
      });

    menu.AddSubMenu("The empty menu");
  }, 
  onlyExitWhenPressingQuit: true);
```


