using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AlbumController(IAlbumRepository albumRepository) : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository = albumRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbumsAsync()
        {
            var albums = await _albumRepository.GetAlbumsAsync();
            return Ok(albums);
        }

        [HttpGet]
        [Route("Photos/{albumId:int}")]
        public async Task<ActionResult<IEnumerable<PhotoModel>>> GetPhotosAsync(int albumId)
        {
            var photos = await _albumRepository.GetPhotosAsync(albumId);
            return Ok(photos);
        }

        [HttpGet("{albumId:int}")]
        public async Task<ActionResult<AlbumModel>> GetAlbumAsync(int albumId)
        {
            var album = await _albumRepository.GetAlbumAsync(albumId);

            return album == null
                ? NotFound()
                : Ok(album);
        }

        [HttpDelete("{albumId:int}")]
        public async Task<ActionResult<bool>> DeleteAlbumAsync(int albumId)
        {
            bool deleted = await _albumRepository.DeleteAlbumAsync(albumId);

            return deleted
                ? Ok(true)
                : Ok(false);
        }
    }
}
