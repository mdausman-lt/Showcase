using Microsoft.AspNetCore.Components.Authorization;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace PhotoAlbum.Web.Services
{
    public class AlbumService : ServiceBase, IAlbumService
    {
        public AlbumService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<AlbumModel>> GetAlbumsAsync()
        {
            var response = await HttpClientGetAsync("api/Album");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            var albums = await response.Content.ReadFromJsonAsync<IEnumerable<AlbumModel>>();
            return albums;
        }

        public async Task<IEnumerable<PhotoModel>> GetPhotosAsync(int albumId)
        {
            var response = await HttpClientGetAsync($"api/Album/Photos/{albumId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            var photos = await response.Content.ReadFromJsonAsync<IEnumerable<PhotoModel>>();
            return photos;
        }

        public async Task<AlbumModel> GetAlbumAsync(int albumId)
        {
            var response = await HttpClientGetAsync($"api/Album/{albumId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            var album = await response.Content.ReadFromJsonAsync<AlbumModel>();
            return album;
        }

        public async Task<bool> DeleteAlbumAsync(int albumId)
        {
            var response = await HttpClientDeleteAsync($"api/Album/{albumId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}
