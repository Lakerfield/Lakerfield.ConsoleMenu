using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lakerfield.ConsoleMenu
{
  public static class ConsoleMenuExtensions
  {
    /// <summary>
    /// Add menu item from class which implements IConsoleTask 
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="task"></param>
    /// <param name="waitForCompletion"></param>
    /// <returns></returns>
    public static ConsoleMenu Add<T>(this ConsoleMenu menu, bool waitForCompletion = true, string overrideTitle = null, ConsoleKey? overrideKey = null) where T : IConsoleTask
    {
      return menu.Add(Activator.CreateInstance<T>(), waitForCompletion, overrideTitle, overrideKey);
    }

    /// <summary>
    /// Add menu item from class which implements IConsoleTask 
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="task"></param>
    /// <param name="waitForCompletion"></param>
    /// <returns></returns>
    public static ConsoleMenu Add(this ConsoleMenu menu, IConsoleTask task, bool waitForCompletion = true, string overrideTitle = null, ConsoleKey? overrideKey = null)
    {
      var key = overrideKey ?? ConsoleKey.A + menu.ItemCount;
      var title = overrideTitle ?? task.Title;
      if (string.IsNullOrWhiteSpace(title))
        title = task.GetType().Name.FromCamelCaseToSentence();

      var item = new ConsoleMenuItem(
        key,
        title,
        () => menu.RunTask(task.Execute()),
        waitForCompletion);
      menu.Add(item);
      return menu;
    }

    /// <summary>
    /// Add custom task
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="key"></param>
    /// <param name="title"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static ConsoleMenu Add(this ConsoleMenu menu, ConsoleKey key, string title, Func<Task> action, bool waitForCompletion = true)
    {
      var item = new ConsoleMenuItem(key, title, action, waitForCompletion);
      menu.Add(item);
      return menu;
    }

    public static void AddSubMenu(this ConsoleMenu menu, string title = "Sub Menu", Action<ConsoleMenu> addMenuItems = null)
    {
      var subMenu = new ConsoleMenu(
        menu.WaitForTask,
        menu.CancellationToken,
        title);

      addMenuItems?.Invoke(subMenu);

      menu.Add(new ConsoleTask(title, () => menu.RunSubMenu(subMenu)));
    }



    /// <summary>
    /// https://stackoverflow.com/questions/323314/best-way-to-convert-pascal-case-to-a-sentence/51310790#51310790
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string FromCamelCaseToSentence(this string input)
    {
      if (string.IsNullOrEmpty(input)) return input;

      var sb = new StringBuilder();
      // start with the first character -- consistent camelcase and pascal case
      sb.Append(char.ToUpper(input[0]));

      // march through the rest of it
      for (var i = 1; i < input.Length; i++)
      {
        // any time we hit an uppercase OR number, it's a new word
        if (char.IsUpper(input[i]) || char.IsDigit(input[i])) sb.Append(' ');
        // add regularly
        sb.Append(input[i]);
      }

      return sb.ToString();
    }
  }
}
