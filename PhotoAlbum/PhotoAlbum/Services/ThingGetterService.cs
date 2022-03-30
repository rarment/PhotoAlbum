using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using System.Configuration;

namespace PhotoAlbum.Services
{
    public interface IThingGetterService
    {
        Task<IEnumerable<AlbumEntry>> GetAllAlbumEntries();
        Task<IEnumerable<AlbumEntry>> GetAlbumEntriesByAlbumId(int id);

    }
    public class ThingGetterService : IThingGetterService
    {
        private const string url = "https://jsonplaceholder.typicode.com/photos";

        public ThingGetterService()
        {
            
        }
        
        public Task<IEnumerable<AlbumEntry>> GetAllAlbumEntries()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AlbumEntry>> GetAlbumEntriesByAlbumId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}