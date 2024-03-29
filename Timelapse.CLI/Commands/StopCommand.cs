﻿using Spectre.Console.Cli;
using System.ComponentModel;
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
            [Description("A commentary to add to the tracking period")]
            [CommandArgument(0, "[Commentary]")]
            public string? Commentary { get; set; } = default!;


            [Description("A date to manually stop the item (must be older than the start of tracking period of the item)")]
            [CommandOption("-d|--date")]
            [DefaultValue("")]
            public string Date { get; init; } = default!;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var period = await _periodService.GetLastRunningPeriod(default);

            if (period is null)
            {
                return 0;
            }

            var stopedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(settings.Date))
            {
                if (!DateTime.TryParse(settings.Date, out stopedAt))
                {
                    throw new FormatException("Could not parse the given date");
                }

                if (stopedAt.ToUniversalTime() > DateTime.UtcNow)
                {
                    ErrorView.Show("You cannot stop an item in a future date");
                    return -1;
                }
            }

            if (stopedAt.ToUniversalTime() <= period.StartedAt)
            {
                ErrorView.Show("You cannot stop an item with a date older then the start tracking period");
                return 0;
            }

            period.StoppedAt = stopedAt.ToUniversalTime();
            period.Commentary = settings.Commentary;

            period = await _periodService.Update(period, default);

            TableView.Show(period.Item);
            return 0;
        }
    }
}
