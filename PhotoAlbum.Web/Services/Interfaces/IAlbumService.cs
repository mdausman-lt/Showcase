using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Web.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumModel>> GetAlbumsAsync();
        Task<IEnumerable<PhotoModel>> GetPhotosAsync(int albumId);
        Task<AlbumModel> GetAlbumAsync(int albumId);
        Task<bool> DeleteAlbumAsync(int albumId);
    }
}
