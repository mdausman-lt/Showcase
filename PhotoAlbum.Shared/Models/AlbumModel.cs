
namespace PhotoAlbum.Shared.Models
{
    public record AlbumModel
    {
        public int AlbumId { get; set; }

        public string Name => string.Concat("Volume ", AlbumId.ToString());
    }
}
