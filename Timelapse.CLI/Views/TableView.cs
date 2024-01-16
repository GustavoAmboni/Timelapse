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
                var duration = period.GetDuration();

                table.AddRow(
                    (i + 1).ToString(),
                    period.Item.HasAnchor() ?
                        $"[link={period.Item.Anchor}]{period.Item.Name}[/]"
                        : period.Item.Name,
                    period.StartedAt.ToLocalTime().ToString("g"),
                    period.StoppedAt?.ToLocalTime().ToString("g") ?? "-",
                    duration.ToString(duration.Days > 0 ?
                        @"%d'd 'hh\:mm" : @"hh\:mm\:ss"),
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
                "Total Duration");

            for (var i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);

                var totalDuration = item.GetTotalDuration();

                table.AddRow(
                    (i + 1).ToString(),

                    item.HasAnchor() ?
                        $"[link={item.Anchor}]{item.Name}[/]"
                        : item.Name,

                    item.IsRunning() ? "Running" : "Stopped",
                    totalDuration.ToString(totalDuration.Days > 0 ? 
                        @"%d'd 'hh\:mm" : @"hh\:mm\:ss")
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
