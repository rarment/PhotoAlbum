using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Models
{
    public class AlbumEntry
    {
        public int albumId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }

        public override string ToString()
        {
            return $"[{id}] {title}";
        }
    }
}