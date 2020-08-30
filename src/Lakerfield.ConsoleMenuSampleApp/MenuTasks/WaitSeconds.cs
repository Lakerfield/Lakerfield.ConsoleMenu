using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lakerfield.ConsoleMenu;

namespace ConsoleMenuSampleApp.MenuTasks
{
  public class WaitSeconds: IConsoleTask
  {
    private readonly int _secondsToWait;

    public string Title => $"Wait for {_secondsToWait} seconds";

    public WaitSeconds(int secondsToWait)
    {
      _secondsToWait = secondsToWait;
    }

    public async Task Execute()
    {
      Console.WriteLine($"Start waiting for {_secondsToWait} seconds");
      await Task.Delay(TimeSpan.FromSeconds(_secondsToWait));
      Console.WriteLine($"Wait for {_secondsToWait} seconds completed");
    }
  }
}
