using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;

namespace PhotoAlbum.Web.Extensions
{
    public static class StorageServiceExtension
    {
        public static async Task SetEncryptedItemAsync<T>(this ILocalStorageService localStorageService, string key, T item)
        {
            var itemJson = JsonSerializer.Serialize(item);
            var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
            var base64Json = Convert.ToBase64String(itemJsonBytes);

            await localStorageService.SetItemAsync(key, base64Json);
        }

        public static async Task<T?> GetEncryptedItemAsync<T>(this ILocalStorageService localStorageService, string key)
        {
            var base64Json = await localStorageService.GetItemAsync<string>(key);
            if (base64Json == null)
            {
                return default;
            }
            var itemJsonBytes = Convert.FromBase64String(base64Json);
            var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
            var item = JsonSerializer.Deserialize<T>(itemJson);

            return item;
        }
    }
}
