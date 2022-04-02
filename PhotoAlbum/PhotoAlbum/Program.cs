using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using PhotoAlbum.Models;
using PhotoAlbum.Repos;
using PhotoAlbum.Services;

class Program
{

    static async Task Main(string[] args)
    {
        var host = CreateHost();

        var service = host.Services.GetRequiredService<IThingGetterService>();
        
        var albums = new List<AlbumGroups>();

        if (args.Length == 0 || args[0] == "-h")
        {
            Console.WriteLine("Please enter the album Id you wish to retrieve.  Leave blank to retrieve all");
            var input = Console.ReadLine();

            albums = await service.GetAlbums(input);
            
            DisplayOutput(albums);
        }
    }
    
    private static IHost CreateHost()
    {
        var host = CreateHostBuilder().Build();
        ConfigureLogger();
        return host;
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                RegisterServices(services, hostContext.Configuration);
            })
            .UseSerilog();
    }

    private static void ConfigureLogger()
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        Log.Logger = loggerConfig.CreateLogger();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IThingGetterService, ThingGetterService>();
        services.AddSingleton<IThingGetterRepo, ThingGetterRepo>();
        services.AddHttpClient<IThingGetterRepo, ThingGetterRepo>();

        var appSettings = new AppSettings();

        config.Bind(appSettings);

        services.AddSingleton(appSettings);
    }

    private static void DisplayOutput(List<AlbumGroups> albumGroups)
    {
        foreach (var album in albumGroups)
        {
            Console.WriteLine($"photo-album {album.AlbumId}");
            foreach (var entries in album.AlbumEntries)
            {
                Console.WriteLine(entries.ToString());
            }
        }
    }
}
