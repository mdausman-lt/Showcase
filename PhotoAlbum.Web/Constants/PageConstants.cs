namespace PhotoAlbum.Web.Constants
{
    public static class PageConstants
    {
        public const string Home = "/";
        public const string Login = "/login";

        public const string MyProfile = "/my/profile";
        public const string MyChangePassword = "/my/changePassword";

        public const string AdminAlbumName = "Album";
        public const string AdminAlbum = "/admin/album";
        public const string AdminAlbums = "/admin/albums";
        public const string AdminAlbumActionId = "/admin/album/{action}/{albumId:int}";

        public const int DefaultPageSize = 10;
    }
}
