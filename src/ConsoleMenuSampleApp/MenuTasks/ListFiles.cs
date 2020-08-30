using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lakerfield.ConsoleMenu;

namespace ConsoleMenuSampleApp.MenuTasks
{
  public class ListFiles : IConsoleTask
  {
    public string Title { get; }
    public Task Execute()
    {
      var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
      var files = directoryInfo.GetFiles();
      foreach (var file in files)
        Console.WriteLine($"- {file.Name}");
      return Task.CompletedTask;
    }
  }
}
