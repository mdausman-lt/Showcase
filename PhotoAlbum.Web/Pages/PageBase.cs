using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PhotoAlbum.Web.Constants;
using PhotoAlbum.Web.Containers;
using PhotoAlbum.Web.Enums;
using PhotoAlbum.Web.Shared;

namespace PhotoAlbum.Web.Pages
{
    public class PageBase : ComponentBase
    {
        [Parameter] public string Action { get; set; }

        [Inject]
        internal ILocalStorageService localStorageService { get; set; }
        [Inject] 
        internal NavigationManager navigationManager { get; set; }
        [Inject] 
        internal MessageContainer messageContainer { get; set; }

        internal ToastDisplay toastDisplay { get; set; }
        
        internal string? errorMessage;
        internal int pageSize = PageConstants.DefaultPageSize;
        internal int page = 1;
        internal string? filter;

        internal async Task ShowInfoMessageAsync(string message)
        {
            if (toastDisplay == null)
                messageContainer.SetMessage(MessageTypeEnum.Information, message);
            else
                await toastDisplay.ShowAsync(MessageTypeEnum.Information, message);
        }

        internal async Task ShowInfoMessageAsync(string page, string name, string action)
        {
            await ShowInfoMessageAsync(string.Format("{0} '{1}' has been {2}.", page, name, action));
        }
    }
}
