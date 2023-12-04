using Microsoft.AspNetCore.Components.Authorization;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace PhotoAlbum.Web.Services
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<UserModel?> GetUserAsync(int userId)
        {
            var response = await HttpClientGetAsync($"api/User/{userId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return user;
        }

        public async Task<TokenModel?> LoginAsync(LoginModel loginModel)
        {
            var response = await HttpClientPostAsJsonAsync("/api/User/Login", loginModel, true);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<TokenModel>()
                : null;
        }

        public async Task<bool> PasswordIsValidAsync(int userId, string password)
        {
            var response = await HttpClientGetAsync($"api/User/{userId}/ValidatePassword/{password}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(ErrorConstants.UnauthorizedAccess);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
