using Spectre.Console;
using Timelapse.CLI.Entities;

namespace Timelapse.CLI.Views
{
    internal static class TableView
    {
        public static void Show(ICollection<Item> items)
        {
            var table = new Table();

            table.AddColumns(
                "#",
                "Name",
                "Status",
                "Duration");

            foreach (var result in items.Select((value, i) => new { Index = i, Value = value }))
            {
                table.AddRow(
                    result.Index.ToString(),
                    result.Value.Name,
                    result.Value.IsRunning() ? "Running" : "Stopped",
                    result.Value.GetTotalDuration().ToString("g")
                );
            }

            AnsiConsole.Write(table);
        }

        public static void Show(Item item)
        {
            Show([item]);
        }
    }
}
