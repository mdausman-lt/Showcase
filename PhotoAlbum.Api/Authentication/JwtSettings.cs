namespace PhotoAlbum.Api.Authentication
{
    public record JwtSettings
    {
        public required string Key { get; init; }
        public int TokenMinutes { get; init; }

    }
}
