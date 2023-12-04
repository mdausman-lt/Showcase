using Microsoft.AspNetCore.Components.Authorization;
using PhotoAlbum.Web.Providers;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PhotoAlbum.Web.Services
{
    public class ServiceBase
    {
        private protected HttpClient _httpClient;
        private protected AuthenticationStateProvider _authenticationStateProvider;

        private protected async Task<HttpResponseMessage> HttpClientGetAsync(string? requestUri, bool anonymous = false)
        {
            return (anonymous || await AttachAuthorizationAsync())
                ? await _httpClient.GetAsync(requestUri)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private protected async Task<HttpResponseMessage> HttpClientPostAsJsonAsync<T>(string? requestUri, T value, bool anonymous = false)
        {
            return (anonymous || await AttachAuthorizationAsync())
                ? await _httpClient.PostAsJsonAsync(requestUri, value)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private protected async Task<HttpResponseMessage> HttpClientDeleteAsync(string? requestUri)
        {
            return await AttachAuthorizationAsync()
                ? await _httpClient.DeleteAsync(requestUri)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private async Task<bool> AttachAuthorizationAsync()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }

            return false;
        }
    }
}
