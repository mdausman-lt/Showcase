using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<AlbumModel>> GetAlbumsAsync();
        Task<IEnumerable<PhotoModel>> GetPhotosAsync(int albumId);
        Task<AlbumModel?> GetAlbumAsync(int albumId);
        Task<bool> DeleteAlbumAsync(int albumId);

    }
}
