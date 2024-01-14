using Spectre.Console.Cli;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Views;

namespace Timelapse.CLI.Commands
{
    internal sealed class StopCommand(IPeriodService periodService)
        : AsyncCommand<StopCommand.Settings>
    {
        private readonly IPeriodService _periodService = periodService;

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[Commentary]")]
            public string? Commentary { get; set; } = default!;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var period = await _periodService.GetLastRunningPeriod(default);

            if(period is null)
            {
                return 0;
            }

            period.StoppedAt = DateTime.UtcNow;

            period = await _periodService.Update(period, default);

            TableView.Show(period.Item);

            return 0;
        }
    }
}
