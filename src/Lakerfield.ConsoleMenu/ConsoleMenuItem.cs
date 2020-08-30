using System;
using System.Threading.Tasks;

namespace Lakerfield.ConsoleMenu
{
  public class ConsoleMenuItem
  {
    public ConsoleKey Key { get; }
    public Func<Task> Function { get; }
    public string Description { get; }
    public bool WaitForCompletion { get; }

    public ConsoleMenuItem(ConsoleKey key, string description, Func<Task> function, bool waitForCompletion)
    {
      Key = key;
      Description = description;
      Function = function;
      WaitForCompletion = waitForCompletion;
    }
  }
}
