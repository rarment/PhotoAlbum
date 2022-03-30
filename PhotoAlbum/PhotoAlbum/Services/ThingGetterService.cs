using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace PhotoAlbum.Services
{
    public interface IThingGetterService
    {
        Task<List<AlbumEntry>> GetAllAlbumEntries(string url);
        Task<List<AlbumEntry>> GetAlbumEntriesByAlbumId(string url, int id);

    }
    public class ThingGetterService : IThingGetterService
    {
        public ThingGetterService()
        {
            
        }

        public async Task<List<AlbumEntry>> GetAllAlbumEntries(string url)
        {
            try
            {
                var entries = await new HttpClient().GetFromJsonAsync<List<AlbumEntry>>(url);
                var groupedEntries = entries.GroupBy(x => x.albumId).Select(y => y.ToList());
                return entries;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred", e);
                throw;
            }
        }

        public async Task<List<AlbumEntry>> GetAlbumEntriesByAlbumId(string url, int id)
        {
            try
            {
                var entries = await new HttpClient().GetFromJsonAsync<List<AlbumEntry>>($"{url}?albumId={id}");
                return entries;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error ocurred", e);
                throw;
            }
        }
    }
}