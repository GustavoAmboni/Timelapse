using Spectre.Console.Cli;
using System.ComponentModel;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Entities;
using Timelapse.CLI.Views;

namespace Timelapse.CLI.Commands
{
    internal sealed class StartCommand(IItemService itemService, IPeriodService periodService) : AsyncCommand<StartCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<Name>")]
            public string Name { get; init; } = default!;

            [CommandArgument(0, "[Description]")]
            [DefaultValue("")]
            public string Description { get; init; } = default!;

            [CommandOption("-l|--link|-a|--anchor")]
            [DefaultValue("")]
            public string Anchor { get; init; } = default!;

        }

        private readonly IItemService _itemService = itemService;
        private readonly IPeriodService _periodService = periodService;

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var item = await _itemService.GetAsNoTracking(settings.Name, default);

            if (item is null)
            {
                item = new Item
                {
                    Name = settings.Name,
                    Description = settings.Description,
                    Anchor = settings.Anchor,
                };

                item = await _itemService.Add(item, default);
            }
            else
            {
                if (await _itemService.IsRunning(settings.Name, default))
                {
                    TableView.Show(item);
                    return 0;
                }
            }

            var period = new Period
            {
                StartedAt = DateTime.UtcNow,
                ItemId = item.ItemId,
            };
            await _periodService.Add(period, default);

            TableView.Show(item);
            return 0;
        }
    }
}
