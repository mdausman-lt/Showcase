using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Api.Entities
{
    public record Photo
    {
        [Key]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public required string Title { get; set; }
        public required string Url { get; set; }
        public required string ThumbnailUrl { get; set; }
    }
}
