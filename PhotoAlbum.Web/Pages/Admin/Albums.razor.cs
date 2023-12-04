using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace PhotoAlbum.Web.Pages.Admin
{
    partial class Albums : PageBase
    {
        [Inject]
        private IAlbumService categoryService { get; set; }

        private IEnumerable<AlbumModel>? albums;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                albums = await categoryService.GetAlbumsAsync();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
