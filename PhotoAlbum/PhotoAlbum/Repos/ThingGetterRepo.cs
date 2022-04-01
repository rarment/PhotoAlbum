using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PhotoAlbum.Models;

namespace PhotoAlbum.Repos
{
    public interface IThingGetterRepo
    {
        Task<List<AlbumEntry>> GetAlbumsAsync(string url, int id = -1);
    }
    public class ThingGetterRepo : IThingGetterRepo
    {
        private readonly HttpClient _client;

        public ThingGetterRepo(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<AlbumEntry>> GetAlbumsAsync(string url, int id = -1)
        {
            var param = id == -1 ? "" : $"?albumId={id}";

            var entries = await _client.GetFromJsonAsync<List<AlbumEntry>>($"{url}{param}");

            return entries;

        }
    }
}