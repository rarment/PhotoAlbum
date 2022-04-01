using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using System.Net.Http;
using System.Net.Http.Json;
using PhotoAlbum.Repos;

namespace PhotoAlbum.Services
{
    public interface IThingGetterService
    {
        Task<List<AlbumGroups>> GetAllAlbumEntries();
        Task<List<AlbumGroups>> GetAlbumEntriesByAlbumId(int id);

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

        public async Task<List<AlbumGroups>> GetAllAlbumEntries()
        {
            try
            {
                var entries = await _thingGetterRepo.GetAlbumsAsync(_appSettings.PhotoAlbumServiceUrl);
                var groupedEntries = entries.GroupBy(x => x.albumId,
                    (i, enumerable) => new AlbumGroups { AlbumEntries = enumerable.ToList(), AlbumId = i }).ToList();
                return groupedEntries;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred", e);
                throw;
            }
        }

        public async Task<List<AlbumGroups>> GetAlbumEntriesByAlbumId(int id)
        {
            var albumGroup = new List<AlbumGroups>();
            try
            {
                var entries = await _thingGetterRepo.GetAlbumsAsync(_appSettings.PhotoAlbumServiceUrl, id);
                albumGroup.Add(new AlbumGroups
                {
                    AlbumId = id,
                    AlbumEntries = entries
                });
                return albumGroup;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error ocurred", e);
                throw;
            }
        }
    }
}