using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace PhotoAlbum.Web.Pages.Admin
{
    partial class AlbumCrud : PageBase
    {
        [Parameter]
        public int AlbumId { get; set; }

        [Inject]
        private IAlbumService albumService { get; set; }
        private AlbumModel? album;

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                album = await albumService.GetAlbumAsync(AlbumId);
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

        protected async Task DeleteAsync()
        {
            _ = await albumService.DeleteAlbumAsync(AlbumId);

            await ShowInfoMessageAsync(PageConstants.AdminAlbumName, album.Name, ActionConstants.Deleted);
            Cancel();
        }

        protected void Cancel()
        {
            navigationManager.NavigateTo(PageConstants.AdminAlbums);
        }
    }
}
