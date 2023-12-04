using Microsoft.AspNetCore.Mvc;
using Moq;
using PhotoAlbum.Api.Controllers;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Models;
using Xunit;

namespace PhotoAlbum.Test
{
    public class AlbumControllerTest
    {
        [Fact]
        public async Task GetAlbumsAsync()
        {
            var data = new List<AlbumModel>
            {
                new() { AlbumId = 1},
                new() { AlbumId = 2},
                new() { AlbumId = 3}
            };

            var albumRepository = new Mock<IAlbumRepository>();
            albumRepository.Setup(x => x.GetAlbumsAsync()).ReturnsAsync(data);

            var albumController = new AlbumController(albumRepository.Object);

            var actionResult = await albumController.GetAlbumsAsync();
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var albums = result.Value as List<AlbumModel>;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(albums);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(albums.Count == 3);
        }

        [Fact]
        public async Task GetPhotosAsync()
        {
            var data = new List<PhotoModel>
            {
                new() { Id = 1, AlbumId = 1, Title = "Photo 1", Url = string.Empty, ThumbnailUrl = string.Empty },
                new() { Id = 2, AlbumId = 1, Title = "Photo 2", Url = string.Empty, ThumbnailUrl = string.Empty },
                new() { Id = 3, AlbumId = 1, Title = "Photo 3", Url = string.Empty, ThumbnailUrl = string.Empty },
            };

            var albumRepository = new Mock<IAlbumRepository>();
            albumRepository.Setup(x => x.GetPhotosAsync(1)).ReturnsAsync(data);
            albumRepository.Setup(x => x.GetPhotosAsync(0)).ReturnsAsync(new List<PhotoModel>());

            var albumController = new AlbumController(albumRepository.Object);

            // Get all photos for album 1
            var actionResult = await albumController.GetPhotosAsync(1);
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var photos = result.Value as List<PhotoModel>;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(photos);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(photos.Count == 3);

            // Get all photos for album 0
            actionResult = await albumController.GetPhotosAsync(0);
            result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            photos = result.Value as List<PhotoModel>;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(photos);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(photos.Count == 0);
        }

        [Fact]
        public async Task GetAlbumAsync()
        {
            var data = new AlbumModel { AlbumId = 1 };

            var albumRepository = new Mock<IAlbumRepository>();
            albumRepository.Setup(x => x.GetAlbumAsync(1)).ReturnsAsync(data);

            var albumController = new AlbumController(albumRepository.Object);

            // Get album 1
            var actionResult = await albumController.GetAlbumAsync(1);
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var album = result.Value as AlbumModel;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(album);

            // Get album 0
            actionResult = await albumController.GetAlbumAsync(0);
            result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(result);
        }

        [Fact]
        public async Task DeleteAlbumAsync()
        {
            var albumRepository = new Mock<IAlbumRepository>();
            albumRepository.Setup(x => x.DeleteAlbumAsync(1)).ReturnsAsync(true);

            var albumController = new AlbumController(albumRepository.Object);

            // Delete album 1
            var actionResult = await albumController.DeleteAlbumAsync(1);
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var deleted = result.Value as bool?;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(deleted);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(deleted);

            // Delete album 0
            actionResult = await albumController.DeleteAlbumAsync(0);
            result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            deleted = result.Value as bool?;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(deleted);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(deleted);
        }
    }
}
