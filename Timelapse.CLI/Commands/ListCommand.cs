using Spectre.Console.Cli;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;

namespace Timelapse.CLI.Commands
{
    internal sealed class ListCommand : AsyncCommand<ListCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[Running]")]
            public bool Running { get; set; } = default!;
        }

        private readonly IItemService _itemService;
        private readonly IPeriodService _periodService;

        public ListCommand(IItemService itemService, IPeriodService periodService)
        {
            _itemService = itemService;
            _periodService = periodService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            throw new NotImplementedException();
        }
    }
}
