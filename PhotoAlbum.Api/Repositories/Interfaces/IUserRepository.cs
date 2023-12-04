using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel?> GetUserAsync(int userId);
        Task<UserModel?> AuthenticateUserAsync(string username, string password);
        Task<bool> PasswordIsValidAsync(int userId, string password);
    }
}
