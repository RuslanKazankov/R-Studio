using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using R_StudioAPI.Config;
using R_StudioAPI.Models;
using R_StudioAPI.Repository;
using R_StudioAPI.Services;
using System.Net;

namespace R_StudioAPI.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationConfig _appConfig;
        private readonly IMediaService _mediaService;
        private readonly IPostMediaRepository _postMediaRepository;
        public MediaController(IOptions<ApplicationConfig> appConfig, IMediaService mediaService, IPostMediaRepository postMediaRepository)
        {
            _appConfig = appConfig.Value;
            _mediaService = mediaService;
            _postMediaRepository = postMediaRepository;
        }

        [HttpGet("postmedia/{filename:string}")]
        public async Task<IActionResult> GetPostMedia(string filename)
        {
            string directoryPath = $"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}";
            string filePath = $"{directoryPath}{filename}";

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest($"File {filename} not exists!");
            }

            string? mediatype = _mediaService.TypeFromMediaExtension(Path.GetExtension(filename));

            if (mediatype == null)
            {
                return BadRequest("Format is not correct!");
            }

            byte[] file = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(file, mediatype);
        }

        [HttpGet("postmedia/{id:long}")]
        public async Task<IActionResult> GetPostMedia(long id)
        {
            PostMedia? postMedia = _postMediaRepository.Get(id);

            if (postMedia == null)
            {
                return BadRequest("File not found");
            }

            string filename = postMedia.Url;

            return await GetPostMedia(filename);
        }
    }
}
