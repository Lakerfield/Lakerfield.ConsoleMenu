using System;
using System.Threading.Tasks;
using ConsoleMenuSampleApp.MenuTasks;
using Lakerfield.ConsoleMenu;

namespace ConsoleMenuSampleApp
{
  class Program
  {
    static async Task Main(string[] args)
    {
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
    }
  }
}
