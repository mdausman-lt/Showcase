
namespace PhotoAlbum.Shared.Models
{
    public record TokenModel
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
    }
}
