using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lakerfield.ConsoleMenu;

namespace Lakerfield.ConsoleMenu
{
  public class ConsoleTask : IConsoleTask
  {
    private readonly Func<Task> _execute;
    public string Title { get; }

    public ConsoleTask(string title, Func<Task> execute)
    {
      _execute = execute;
      Title = title;
    }

    public Task Execute()
    {
      return _execute();
    }
  }
}
