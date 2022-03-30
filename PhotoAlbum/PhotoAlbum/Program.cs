using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoAlbum.Models;
using PhotoAlbum.Services;

class Program
{
    private static string _baseUrl = "";
    
    static void Main(string[] args)
    {

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _baseUrl = config.GetSection("PhotoAlbumServiceUrl").Value;
        var services = new ServiceCollection();
        services
            .AddTransient<IThingGetterService, ThingGetterService>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var thingGetterService = serviceProvider.GetService<IThingGetterService>();
        var entries = new List<AlbumEntry>();

        if (args.Length == 0 || args[0] == "-h")
        {
            Console.WriteLine("Please enter the album Id you wish to retrieve.  Leave blank to retrieve all");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                entries = thingGetterService.GetAllAlbumEntries(_baseUrl)
                    .Result;

                foreach (var entry in entries)
                {
                    Console.WriteLine(entry.ToString());
                }

            }
            else
            {
                var albumId = int.Parse(input);
                entries = thingGetterService
                    .GetAlbumEntriesByAlbumId(_baseUrl, albumId).Result;

                foreach (var entry in entries)
                {
                    Console.WriteLine(entry.ToString());
                }
            }
        }
    }
}
