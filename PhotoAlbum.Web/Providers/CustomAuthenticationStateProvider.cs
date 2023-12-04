using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Extensions;
using System.Security.Claims;

namespace PhotoAlbum.Web.Providers
{
    public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorageService.GetEncryptedItemAsync<TokenModel>(LocalStorageConstants.Token);

                if (token == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Name, token.Username),
                    new(ClaimTypes.Role, token.Role)
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationStateAsync(TokenModel? token)
        {
            ClaimsPrincipal claimsPrincipal;

            if (token != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Name, token.Username),
                    new(ClaimTypes.Role, token.Role)
                }));

                await UpdateTokenExpirationAsync(token);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _localStorageService.RemoveItemAsync(LocalStorageConstants.Token);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetTokenAsync()
        {
            var result = string.Empty;

            try
            {
                TokenModel? token = await _localStorageService.GetEncryptedItemAsync<TokenModel>(LocalStorageConstants.Token);

                if (token != null && DateTime.UtcNow < token.ExpiryTimeStamp)
                {
                    await UpdateTokenExpirationAsync(token);

                    result = token.Token;
                }
            }
            catch { }

            return result;
        }

        private async Task UpdateTokenExpirationAsync(TokenModel token)
        {
            token.ExpiryTimeStamp = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
            await _localStorageService.SetEncryptedItemAsync(LocalStorageConstants.Token, token);
        }
    }
}
