using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Shared.Models
{
    public record LoginModel
    {
        public LoginModel() 
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
