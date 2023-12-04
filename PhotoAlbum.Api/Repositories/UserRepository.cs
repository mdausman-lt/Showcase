using PhotoAlbum.Api.Entities;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Enums;
using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return UserQuery()
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task<UserModel?> GetUserAsync(int userId)
        {
            return UserQuery()
                .FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<UserModel?> AuthenticateUserAsync(string username, string password)
        {
            var user = UserQuery()
                .FirstOrDefault(x => x.Username == username);

            if (user != null)
            {
                if (password.Equals(user.Password))
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<bool> PasswordIsValidAsync(int userId, string password)
        {
            var user = UserQuery().FirstOrDefault(x => x.UserId == userId);

            if (user != null)
            {
                return password.Equals(user.Password);
            }

            return false;
        }

        private IList<UserModel> UserQuery()
        {
            var users = new List<User>
            {
                new() { UserId = 1, Username = "test", Name = "Demo Test", Password = "test", IsAdmin = true },
                new() { UserId = 2, Username = "admin", Name = "Demo Admin", Password = "admin", IsAdmin = true },
                new() { UserId = 3, Username = "user",  Name = "Demo User",  Password = "user",  IsAdmin = false }
            };

            return users
                .Select(x => new UserModel
                {
                    UserId = x.UserId,
                    Username = x.Username,
                    Name = x.Name,
                    Password = x.Password,
                    Role = x.IsAdmin ? RolesEnum.Admin.ToString() : RolesEnum.User.ToString(),

                })
                .ToList();
        }
    }
}
