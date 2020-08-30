using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace Lakerfield.ConsoleMenu
{
  public class ConsoleMenu
  {
    private readonly CancellationToken _parentCancellationToken;
    private readonly Action<Task> _waitForTask;
    private readonly string _description;
    public CancellationToken CancellationToken { get; private set; }
    private readonly List<ConsoleMenuItem> _items;
    public int ItemCount => _items.Count;

    public ConsoleMenu(Action<Task> waitForTask, CancellationToken cancellationToken, string description, params ConsoleMenuItem[] items)
    {
      _waitForTask = waitForTask;
      _parentCancellationToken = cancellationToken;
      _description = description;
      _items = new List<ConsoleMenuItem>(items);
    }

    public void Add(ConsoleMenuItem item)
    {
      _items.Add(item);
    }

    public void WaitForTask(Task task)
    {
      _waitForTask(task);
    }

    public static async Task RunMainMenuAndWaitForCompletion(string title = "Lakerfield Main Console Menu", Action<ConsoleMenu> addMenuItems = null, bool onlyExitWhenPressingQuit = true)
    {
      var cancellationTokenSource = new CancellationTokenSource();
      var cancellationToken = cancellationTokenSource.Token;
      var tasks = new List<Task>();

      var menu = new ConsoleMenu(
        (task) => tasks.Add(task),
        cancellationToken,
        title);

      addMenuItems?.Invoke(menu);

      if (onlyExitWhenPressingQuit)
      {
        menu.Add(ConsoleKey.Q, "Quit", () =>
        {
          cancellationTokenSource.Cancel();
          return Task.CompletedTask;
        });

        while (!cancellationToken.IsCancellationRequested)
        {
          await menu.Run();

          if (cancellationToken.IsCancellationRequested)
            break;
          Console.WriteLine();
          Console.WriteLine($@"Exit prevented, to quit Q must be pressed");
        }
      }
      else
      {
        await menu.Run();
      }

      tasks = tasks.Where(t => !t.IsCompleted).ToList();
      Console.WriteLine($"Waiting for completion of {tasks.Count} tasks...");
      await Task.WhenAll(tasks);
    }

    public async Task Run()
    {
      var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_parentCancellationToken);
      CancellationToken = cancellationTokenSource.Token;

      while (!CancellationToken.IsCancellationRequested)
      {
        if (_items.Any(i => !i.WaitForCompletion))
        {
          Console.WriteLine(string.Join(Environment.NewLine,
            "________________________________________",
            "",
            $"  {_description}",
            "",
            string.Join(Environment.NewLine, _items.Select(i => $"  {i.Key}{(i.WaitForCompletion ? " " : "^")}- {i.Description}")),
            "   ^= Menu option is asynchronously executed",
            "ESC - Go back",
            ""));
        }
        else
        {
          Console.WriteLine(string.Join(Environment.NewLine,
            "________________________________________",
            "",
            $"  {_description}",
            "",
            string.Join(Environment.NewLine, _items.Select(i => $"  {i.Key} - {i.Description}")),
            "ESC - Go back",
            ""));
        }

        var response = Console.ReadKey(true);
        if (response.Key == ConsoleKey.Escape)
        {
          cancellationTokenSource.Cancel();
        }
        else
        {
          var item = _items.FirstOrDefault(i => i.Key == response.Key);
          if (item != null)
          {
            var task = ExecuteLoggedTask(item.Function);
            _waitForTask(task);
            if (item.WaitForCompletion)
              await task;
          }
        }
      }
    }

    public async Task RunTask(Task task)
    {
      WaitForTask(task);
      await task;
    }

    public Task RunSubMenu(ConsoleMenu subMenu)
    {
      return RunTask(subMenu.Run());
    }

    private async Task ExecuteLoggedTask(Func<Task> function)
    {
      try
      {
        await function();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
  }
}
