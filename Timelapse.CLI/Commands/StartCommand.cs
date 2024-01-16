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
            [Description("Name of the item")]
            [CommandArgument(0, "<Name>")]
            public string Name { get; init; } = default!;

            [Description("Description of the item")]
            [CommandArgument(1, "[Description]")]
            [DefaultValue("")]
            public string Description { get; init; } = default!;

            [Description("A link to an web page")]
            [CommandOption("-l|--link|-a|--anchor")]
            [DefaultValue("")]
            public string Anchor { get; init; } = default!;

            [Description("A date to manually start the item (must be older than the last tracking end date of the item)")]
            [CommandOption("-d|--date")]
            [DefaultValue("")]
            public string Date { get; init; } = default!;

        }

        private readonly IItemService _itemService = itemService;
        private readonly IPeriodService _periodService = periodService;

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            if (await _itemService.IsAnyRunning(default))
            {
                var lastRunningPeriod = (await _periodService.GetLastRunningPeriod(default))!;

                ErrorView.Show($"The item \"{lastRunningPeriod.Item.Name}\" is already running. " +
                    "Please stop the running item first to start another one");
                return 0;
            }

            var startedAt = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(settings.Date))
            {
                if (!DateTime.TryParse(settings.Date, out startedAt))
                {
                    ErrorView.Show("Could not parse the given date");
                    return -1;
                }

                if (startedAt.ToUniversalTime() > DateTime.UtcNow)
                {
                    ErrorView.Show("You cannot start an item in a future date");
                    return -1;
                }
            }

            var item = await _itemService.Get(settings.Name, default);

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
                if (startedAt.ToUniversalTime() <= item.Periods.Last().StoppedAt)
                {
                    ErrorView.Show("You cannot start an item with a date older then the last tracking period");
                    return 0;
                }
            }

            var period = new Period
            {
                StartedAt = startedAt.ToUniversalTime(),
                ItemId = item.ItemId,
            };
            await _periodService.Add(period, default);

            TableView.Show(item);
            return 0;
        }
    }
}
