using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using R_StudioAPI.Config;
using R_StudioAPI.Services;
using System.Net;

namespace R_StudioAPI.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationConfig _appConfig;
        public MediaController(IOptions<ApplicationConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        [HttpGet("postmedia/{filename}")]
        public async Task<IActionResult> GetPostMedia([FromServices] IMediaService mediaService, string filename)
        {
            string directoryPath = $"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}";
            string filePath = $"{directoryPath}{filename}";

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest($"File {filename} not exists!");
            }

            string? mediatype = mediaService.TypeFromMediaExtension(Path.GetExtension(filename));

            if (mediatype == null)
            {
                return BadRequest("Format is not correct!");
            }

            byte[] file = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(file, mediatype);
        }

    }
}
