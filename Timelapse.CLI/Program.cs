using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using Timelapse.CLI.Application.ApplicationServices;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Commands;
using Timelapse.CLI.Infraestructure;
using Timelapse.CLI.Infraestructure.Data.Context;

namespace Timelapse.CLI
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            var registrar = new TypeRegistrar(host);

            var app = new CommandApp(registrar);

            app.Configure(config =>
            {
                config.SetApplicationName("Timelapse");
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif

                config.AddCommand<StartCommand>("start")
                    .WithAlias("track")
                    .WithAlias("t")
                    .WithDescription("Creates a new or tracks an old item by the name")
                    .WithExample("start",
                        "\"Take the garbage out\"",
                        "\"The garbage is smelly and needs to be taken out\"")
                    .WithExample("track",
                        "\"Wash the dishes\"",
                        "\"I need to buy a dish washing machine\"",
                        "--link",
                        "https://open.spotify.com")
                    .WithExample("t",
                        "\"Running 10 km\"",
                        "-a",
                        "https://open.spotify.com")
                    .WithExample("t",
                        "\"Swimming for an hour\"",
                        "-a",
                        "https://open.spotify.com",
                        "--date",
                        "19:00")
                    .WithExample("t",
                        "\"Buying clothes\"",
                        "-d",
                        "\"22/01/2023 07:00am\"");

                config.AddCommand<ListCommand>("list")
                    .WithAlias("l")
                    .WithDescription("Lists the last 5 items by default or gets items by a date range")
                    .WithExample("list")
                    .WithExample("list", "--start", "07:00", "--end", "19:00")
                    .WithExample("list", "--start", "07:00am", "--end", "07:00pm")
                    .WithExample("list", "-s", "07:00", "-e", "19:00")
                    .WithExample("l", "-s", "07:00am", "-e", "07:00pm")
                    .WithExample("l", "--start", "07:00")
                    .WithExample("l", "-s", "19:00");

                config.AddCommand<StopCommand>("stop")
                    .WithAlias("finish")
                    .WithAlias("f")
                    .WithDescription("Stops the last running item")
                    .WithExample("stop", "\"Pause for lunch\"")
                    .WithExample("finish", "\"End of the day\"")
                    .WithExample("f", "\"Pause for 10 minutes\"")
                    .WithExample("f", "\"Coffe break\"", "--date", "19:00")
                    .WithExample("f", "\"Pomodoro break\"", "--date", "07:00pm")
                    .WithExample("stop", "\"Be right back\"", "--date", "\"22/04/2023 07:00pm\"");
            });

            await app.RunAsync(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host
              .CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  // add service registrations here
                  services.AddDbContext<ApplicationDbContext>();
                  services.AddScoped<IItemService, ItemService>();
                  services.AddScoped<IPeriodService, PeriodService>();
              });
    }
}
