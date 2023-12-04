
namespace PhotoAlbum.Shared.Models
{
    public record PhotoModel
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public required string Title { get; set; }
        public required string Url { get; set; }
        public required string ThumbnailUrl { get; set; }
    }
}
