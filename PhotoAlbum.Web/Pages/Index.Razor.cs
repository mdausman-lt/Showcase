using Microsoft.AspNetCore.Components;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Enums;
using PhotoAlbum.Shared.Models;
using PhotoAlbum.Web.Services.Interfaces;

namespace PhotoAlbum.Web.Pages
{
    partial class Index : PageBase
    {
        [Inject]
        private IAlbumService albumService { get; set; }

        private IEnumerable<AlbumModel>? albums;
        private IEnumerable<PhotoModel>? photosCache;
        private IEnumerable<PhotoModel>? photos;
        private int albumId;
        private ViewEnum view = ViewEnum.NoRecords;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                int size = await localStorageService.GetItemAsync<int>(LocalStorageConstants.PageSize);
                pageSize = size == 0
                    ? PageConstants.DefaultPageSize
                    : size;

                albums = await albumService.GetAlbumsAsync();
                photos = photosCache = await albumService.GetPhotosAsync(albumId);
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

        protected async Task SelectedAlbumChangedAsync(ChangeEventArgs e)
        {
            if (e.Value != null && int.TryParse(e.Value.ToString(), out albumId))
            {
                photos = photosCache = await albumService.GetPhotosAsync(albumId);
                view = photos.Any()
                    ? view == ViewEnum.NoRecords
                        ? ViewEnum.List
                        : view = view
                    : ViewEnum.NoRecords;

                StateHasChanged();
            }
        }

        protected async Task ViewChangedAsync(ChangeEventArgs e)
        {
            if (e.Value != null && photos.Any())
            {
                switch (e.Value.ToString())
                {
                    case nameof(ViewEnum.List):
                        view = ViewEnum.List;
                        break;
                    case nameof(ViewEnum.Thumbnails):
                        view = ViewEnum.Thumbnails;
                        break;
                    default:
                        view = ViewEnum.NoRecords;
                        break;
                }

                StateHasChanged();
            }
        }

        private void FilterChanged(string value)
        {
            filter = value;
            RefreshPhotos();
            page = 1;
        }

        private void ClearFilter()
        {
            filter = string.Empty;
            RefreshPhotos();
            page = 1;
        }

        private void RefreshPhotos()
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                photos = photosCache;
            }
            else
            {
                photos = photosCache.Where(x => x.Title.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || x.Title.StartsWith(filter, StringComparison.OrdinalIgnoreCase));
            }
        }

        protected void PageClicked(int pageNum)
        {
            page = pageNum;
        }

        protected async Task PageSizeChangedAsync(int pageSize)
        {
            this.pageSize = pageSize;
            await localStorageService.SetItemAsync(LocalStorageConstants.PageSize, pageSize);
        }
    }
}
