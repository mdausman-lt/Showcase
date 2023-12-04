using Microsoft.AspNetCore.Components;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Extensions;
using PhotoAlbum.Web.Services.Interfaces;

namespace PhotoAlbum.Web.Pages.My
{
    partial class Profile : PageBase
    {
        [Inject] private IUserService userService { get; set; }

        private UserModel? user;
        private bool invalidblockHidden { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                LoginModel currentUser = await localStorageService.GetEncryptedItemAsync<LoginModel>(LocalStorageConstants.Token);
                user = await userService.GetUserAsync(currentUser.UserId);
            }
            catch (UnauthorizedAccessException)
            {
                navigationManager.NavigateTo(PageConstants.Login);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        protected async Task SaveAsync()
        {
            await ShowInfoMessageAsync("Your Profile has been saved (not really... this is just a demo).");
        }
    }
}
