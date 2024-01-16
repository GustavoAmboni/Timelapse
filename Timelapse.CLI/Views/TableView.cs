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

            for (var i = 0; i < periods.Count(); i++)
            {
                var period = periods.ElementAt(i);

                table.AddRow(
                    (i + 1).ToString(),
                    period.Item.HasAnchor() ?
                        $"[link={period.Item.Anchor}]{period.Item.Name}[/]"
                        : period.Item.Name,
                    period.StartedAt.ToLocalTime().ToString("g"),
                    period.StoppedAt?.ToLocalTime().ToString("g") ?? "-",
                    period.GetDuration().ToString(@"hh\:mm\:ss"),
                    period.Commentary ?? string.Empty);
            }

            AnsiConsole.Write(table);
        }

        public static void Show(Period period)
        {
            Show([period]);
        }

        public static void Show(IEnumerable<Item> items)
        {
            var table = new Table();

            table.AddColumns(
                "#",
                "Name",
                "Status",
                "Duration");

            for (var i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);

                table.AddRow(
                    (i + 1).ToString(),
                    item.HasAnchor() ?
                        $"[link={item.Anchor}]{item.Name}[/]"
                        : item.Name,
                    item.IsRunning() ? "Running" : "Stopped",
                    item.GetTotalDuration().ToString(@"hh\:mm\:ss")
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
