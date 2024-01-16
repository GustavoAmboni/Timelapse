using Spectre.Console.Cli;
using System.ComponentModel;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Views;

namespace Timelapse.CLI.Commands
{
    internal sealed class ListCommand(IPeriodService periodService) : AsyncCommand<ListCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [Description("A valid date to list items older than this date")]
            [CommandOption("-s|--start")]
            public string? StartPeriod { get; set; }

            [Description("A valid date to list items newer than this date (optional, default set to 1 day newer than the start)")]
            [CommandOption("-e|--end")]
            public string? EndPeriod { get; set; }
        }

        private readonly IPeriodService _periodService = periodService;

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.StartPeriod))
            {
                if (string.IsNullOrWhiteSpace(settings.EndPeriod))
                {
                    TableView.Show(await _periodService.GetAsNoTracking(5, default));
                    return 0;
                }
                else
                {
                    ErrorView.Show("You have to inform the Starting Period when you already informed the End Period");
                    return -1;
                }

            }

            if (!DateTime.TryParse(settings.StartPeriod, out var startPeriod))
                ErrorView.Show("Could not parse the given start period");

            var endPeriod = startPeriod.AddDays(1);

            if (!string.IsNullOrWhiteSpace(settings.EndPeriod))
            {
                if (!DateTime.TryParse(settings.EndPeriod, out endPeriod))
                {
                    ErrorView.Show("Could not parse the given end period");
                    return -1;
                }
            }

            if (endPeriod < startPeriod)
            {
                ErrorView.Show("The End Period cannot be older than the start period");
                return -1;
            }

            var periods = await _periodService
                .GetAsNoTracking(w => w.StartedAt >= startPeriod && w.StoppedAt <= endPeriod, default);

            TableView.Show(periods);
            return 0;
        }
    }
}
