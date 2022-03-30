using System.Collections.Generic;

namespace PhotoAlbum.Models
{
    public class AlbumGroups
    {
        public int AlbumIb { get; set; }
        public List<AlbumEntry> AlbumEntries { get; set; }
    }
}