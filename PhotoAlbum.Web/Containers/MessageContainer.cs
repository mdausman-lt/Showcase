using PhotoAlbum.Web.Enums;
using PhotoAlbum.Web.EventArgs;

namespace PhotoAlbum.Web.Containers
{
    public class MessageContainer
    {
        public event Action<MessageEventArgs> OnMessageChange;

        public void SetMessage(MessageTypeEnum messageType, string message)
        {
            OnMessageChange?.Invoke(new MessageEventArgs
            {
                MessageType = messageType,
                Message = message
            });
        }
    }
}
