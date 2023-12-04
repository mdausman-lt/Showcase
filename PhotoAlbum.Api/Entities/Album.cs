using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Api.Entities
{
    public record Album
    {
        [Key]
        public int Id { get; set; }

        public bool Deleted { get; set; }
    }
}
