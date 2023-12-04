using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel?> GetUserAsync(int userId);
        Task<TokenModel?> LoginAsync(LoginModel loginRequest);
        Task<bool> PasswordIsValidAsync(int userId, string password);
    }
}
