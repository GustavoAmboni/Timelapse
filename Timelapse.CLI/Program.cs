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
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif

                config.AddCommand<StartCommand>("start")
                    .WithAlias("s")
                    .WithExample("start", "Test", "Testing somethig")
                    .WithExample("start", "Test", "Testing somethig", "--link", "http://google.com")
                    .WithExample("s", "Test", "Testing somethig", "-l", "http://google.com");


                config.AddCommand<ListCommand>("list")
                    .WithAlias("l")
                    .WithExample("list");
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
