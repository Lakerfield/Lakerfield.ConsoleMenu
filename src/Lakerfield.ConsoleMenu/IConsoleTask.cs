using System.Threading.Tasks;

namespace Lakerfield.ConsoleMenu
{
  public interface IConsoleTask
  {
    string Title { get; }
    Task Execute();
  }
}