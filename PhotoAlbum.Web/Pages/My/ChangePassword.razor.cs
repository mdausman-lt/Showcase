using Microsoft.AspNetCore.Components;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Extensions;
using PhotoAlbum.Web.Services.Interfaces;

namespace PhotoAlbum.Web.Pages.My
{
    partial class ChangePassword : PageBase
    {
        [Inject] private IUserService userService { get; set; }

        private int userId;
        private bool invalidPasswordHidden = true;
        private bool invalidConfirmHidden = true;
        private string currentPassword;
        private string newPassword;
        private string confirmPassword;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                LoginModel? currentUser = await localStorageService.GetEncryptedItemAsync<LoginModel>(LocalStorageConstants.Token);
                userId = currentUser == null 
                    ? 0 
                    : currentUser.UserId;
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
            invalidConfirmHidden = true;
            invalidPasswordHidden = true;

            if (newPassword != confirmPassword)
            {
                invalidConfirmHidden = false;
            }
            else if (!await userService.PasswordIsValidAsync(userId, currentPassword))
            {
                invalidPasswordHidden = false;
            }
            else
            {
                await ShowInfoMessageAsync("Your password has been updated (again... not really).");
                navigationManager.NavigateTo(PageConstants.MyProfile);
            }
        }
    }
}
