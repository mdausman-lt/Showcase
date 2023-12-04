using Microsoft.Extensions.Caching.Memory;
using PhotoAlbum.Api.Entities;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        // NOTE!!!
        // Caching is used here for this prototype solely for the purpose of acting as a
        // database for deleting purposes and rather than hitting the Interweb every time.

        private enum CacheKeys
        {
            AlbumsCache,
            PhotosCache
        }

        private const int CACHE_EXPIRATION_MINUTES = 20;

        private readonly IMemoryCache _memoryCache;
        private IEnumerable<Album>? _albums;
        private IEnumerable<Photo>? _photos;

        public AlbumRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            Task.Run(RestoreDataAsync).Wait(); 
        }

        public async Task<IEnumerable<AlbumModel>> GetAlbumsAsync()
        {
            return _albums == null
                ? Enumerable.Empty<AlbumModel>()
                : _albums
                    .Where(x => !x.Deleted)
                    .Select(x => new AlbumModel
                    {
                        AlbumId = x.Id
                    })
                    .ToList();
        }

        public async Task<IEnumerable<PhotoModel>> GetPhotosAsync(int albumId)
        {
            return _photos == null
                ? Enumerable.Empty<PhotoModel>()
                : _photos
                    .Where(x => x.AlbumId == albumId)
                    .OrderBy(x => x.Id)
                    .Select(x => new PhotoModel
                    {
                        Id = x.Id,
                        AlbumId = x.AlbumId,
                        Title = x.Title,
                        ThumbnailUrl = x.ThumbnailUrl,
                        Url = x.Url
                    })
                    .ToList();
        }

        public async Task<AlbumModel?> GetAlbumAsync(int albumId)
        {
            if (_albums != null)
            {
                Album? album = _albums.FirstOrDefault(x => !x.Deleted && x.Id == albumId);

                if (album != null)
                    return new AlbumModel
                    {
                        AlbumId = album.Id
                    };
            }

            return null;
        }

        public async Task<bool> DeleteAlbumAsync(int albumId)
        {
            if (_albums != null)
            {
                Album? album = _albums.FirstOrDefault(x => x.Id == albumId);

                if (album == null)
                    return false;

                album.Deleted = true;
                _memoryCache.Set(CacheKeys.AlbumsCache.ToString(), _albums, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES)));

                return true;
            }

            return false;
        }

        private async Task RestoreDataAsync()
        {
            if (!_memoryCache.TryGetValue(CacheKeys.PhotosCache.ToString(), out _photos))
            {
                HttpClient httpClient = new();

                var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/photos");

                response.EnsureSuccessStatusCode();

                _photos = await response.Content.ReadFromJsonAsync<IEnumerable<Photo>>();
                _memoryCache.Set(CacheKeys.PhotosCache.ToString(), _photos, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES)));
            }

            if (!_memoryCache.TryGetValue(CacheKeys.AlbumsCache.ToString(), out _albums))
            {
                _albums = _photos == null
                    ? Enumerable.Empty<Album>()
                    : _photos
                        .GroupBy(x => x.AlbumId)
                        .Select(x => x.First())
                        .Select(x => new Album
                        {
                            Id = x.AlbumId,
                            Deleted = false
                        })
                        .ToList();

                _memoryCache.Set(CacheKeys.AlbumsCache.ToString(), _albums, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES)));
            }
        }
    }
}
