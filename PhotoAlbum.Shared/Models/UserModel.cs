using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Shared.Models
{
    public record UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(128, ErrorMessage = "Maximum Username length is {1}")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(32, ErrorMessage = "Maximum Name length is {1}")]
        public string Name { get; set; }

        public string Password { get; set; }    

        public string Email
        {
            get { return Name.Replace(' ','.').ToLower() + "@photos.com"; }
            set { var e = value; }
        }

        public string Role { get; set; }
    }
}
