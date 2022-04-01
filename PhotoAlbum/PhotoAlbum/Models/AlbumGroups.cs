using System.Collections.Generic;

namespace PhotoAlbum.Models
{
    public class AlbumGroups
    {
        public int AlbumId { get; set; }
        public List<AlbumEntry> AlbumEntries { get; set; }
    }
}