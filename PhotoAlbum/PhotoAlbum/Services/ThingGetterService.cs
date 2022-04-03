using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using PhotoAlbum.Repos;
using Serilog;

namespace PhotoAlbum.Services
{
    public interface IThingGetterService
    {
        Task<List<AlbumGroups>> GetAlbumsAsync(string albumId);
    }
    public class ThingGetterService : IThingGetterService
    {
        private readonly IThingGetterRepo _thingGetterRepo;
        private readonly AppSettings _appSettings;
        
        public ThingGetterService(IThingGetterRepo thingGetterRepo, AppSettings appSettings)
        {
            _thingGetterRepo = thingGetterRepo;
            _appSettings = appSettings;
        }

        private async Task<List<AlbumGroups>> GetAllAlbumEntries()
        {
            try
            {
                ValidateServiceUrl();
                var entries = await _thingGetterRepo.GetAlbumsAsync(_appSettings.PhotoAlbumServiceUrl);
                var groupedEntries = entries.GroupBy(x => x.albumId,
                    (i, enumerable) => new AlbumGroups { AlbumEntries = enumerable.ToList(), AlbumId = i }).ToList();
                Log.Information("GetAllAlbumEntries - Success");
                return groupedEntries;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred", e);
                Log.Error("GetAllAlbumEntries - Error occurred getting album entries", e);
                throw;
            }
        }

        private async Task<List<AlbumGroups>> GetAlbumEntriesByAlbumId(int id)
        {
            var albumGroup = new List<AlbumGroups>();
            try
            {
                ValidateServiceUrl();
                var entries = await _thingGetterRepo.GetAlbumsAsync(_appSettings.PhotoAlbumServiceUrl, id);
                albumGroup.Add(new AlbumGroups
                {
                    AlbumId = id,
                    AlbumEntries = entries
                });
                Log.Information("GetAlbumEntriesByAlbumId - Success");
                return albumGroup;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred", e);
                Log.Error("GetAlbumEntriesByAlbumId - Error getting album entries", e);
                throw;
            }
        }
        
        private void ValidateServiceUrl()
        {
            if (!Uri.IsWellFormedUriString(_appSettings.PhotoAlbumServiceUrl, UriKind.Absolute))
                throw new ArgumentException("Source URL is not valid", nameof(_appSettings.PhotoAlbumServiceUrl));
        }

        public async Task<List<AlbumGroups>> GetAlbumsAsync(string albumId)
        {
            var albums = new List<AlbumGroups>();
            if (string.IsNullOrWhiteSpace(albumId))
            {
                albums = await GetAllAlbumEntries();
            }

            if (int.TryParse(albumId, out var id))
            {
                albums = await GetAlbumEntriesByAlbumId(id);
            }
            else
            {
                Console.WriteLine("Please enter a valid album id (integer)");
            }

            return albums;
        }
    }
}