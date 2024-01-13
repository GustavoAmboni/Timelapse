using Spectre.Console.Cli;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Views;

namespace Timelapse.CLI.Commands
{
    internal sealed class ListCommand(IPeriodService periodService) : AsyncCommand<ListCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[Running]")]
            public bool Running { get; set; } = default!;
        }

        private readonly IPeriodService _periodService = periodService;

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var periods = await _periodService.GetAsNoTracking(5, default);

            TableView.Show(periods);
            return 0;
        }
    }
}
