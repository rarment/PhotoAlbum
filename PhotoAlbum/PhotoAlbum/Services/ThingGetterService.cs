using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace PhotoAlbum.Services
{
    public interface IThingGetterService
    {
        Task<List<AlbumEntry>> GetAllAlbumEntries(string url);
        Task<List<AlbumEntry>> GetAlbumEntriesByAlbumId(string url, int id);

    }
    public class ThingGetterService : IThingGetterService
    {
        private const string url = "https://jsonplaceholder.typicode.com/photos";

        public ThingGetterService()
        {
            
        }

        public async Task<List<AlbumEntry>> GetAllAlbumEntries(string url)
        {
            try
            {
                var response = await new HttpClient().GetFromJsonAsync<List<AlbumEntry>>(url);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error ocurred", e);
                throw;
            }
        }

        public async Task<List<AlbumEntry>> GetAlbumEntriesByAlbumId(string url, int id)
        {
            try
            {
                var response = await new HttpClient().GetFromJsonAsync<List<AlbumEntry>>($"{url}?albumId={id}");
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error ocurred", e);
                throw;
            }
        }
    }
}