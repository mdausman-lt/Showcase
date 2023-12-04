using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Providers;
using PhotoAlbum.Web.Services.Interfaces;

namespace PhotoAlbum.Web.Pages
{
    partial class Login : PageBase
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] private NavigationManager navManager { get; set; }
        [Inject] private IUserService userService { get; set; }

        private readonly LoginModel loginModel = new();
        private bool invalidblockHidden { get; set; } = true;

        private async Task AuthenticateAsync()
        {
            var tokenModel = await userService.LoginAsync(loginModel);

            if (tokenModel == null)
            {
                invalidblockHidden = false;
            }
            else
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                await customAuthStateProvider.UpdateAuthenticationStateAsync(tokenModel);

                navManager.NavigateTo(PageConstants.Home, true);
            }
        }
    }
}
