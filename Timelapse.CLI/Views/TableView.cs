using Spectre.Console;
using Timelapse.CLI.Entities;

namespace Timelapse.CLI.Views
{
    internal static class TableView
    {
        public static void Show(IEnumerable<Period> periods)
        {
            var table = new Table();

            table.AddColumns(
                "#",
                "Name",
                "Started",
                "Stoped",
                "Duration",
                "Comment");

            foreach (var result in periods.Select((value, i) => new { Index = i, Value = value }))
            {
                table.AddRow(
                    result.Index.ToString(),
                    result.Value.Item.Name,
                    result.Value.StartedAt.ToLocalTime().ToString("g"),
                    result.Value.StoppedAt?.ToLocalTime().ToString("g") ?? "-",
                    result.Value.GetDuration().ToString(@"hh\:mm\:ss"),
                    result.Value.Commentary ?? string.Empty);
            }

            AnsiConsole.Write(table);
        }

        public static void Show(IEnumerable<Item> items)
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
                    result.Value.GetTotalDuration().ToString(@"hh\:mm\:ss")
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
