using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Api.Entities
{
    public record User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(128, ErrorMessage = "Maximum Username length is {1}")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(32, ErrorMessage = "Maximum Name length is {1}")]
        public required string Name { get; set; }

        public required string Password { get; set; }

        public bool IsAdmin { get; set; }

        public string Email => Name + "@PhotoAlbum.com";
    }
}
