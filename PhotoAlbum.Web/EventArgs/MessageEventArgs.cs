using PhotoAlbum.Web.Enums;

namespace PhotoAlbum.Web.EventArgs
{
    public record MessageEventArgs
    {
        public MessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }
    }
}
