using Spectre.Console;

namespace Timelapse.CLI.Views
{
    internal static class ErrorView
    {
        public static void Show(string message)
        {
            AnsiConsole.MarkupLine($"[Red]Error:[/] {message}");
        }
    }
}
