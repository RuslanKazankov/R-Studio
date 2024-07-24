using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using R_StudioAPI.Config;
using R_StudioAPI.Data;
using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Extensions;
using R_StudioAPI.Models;
using R_StudioAPI.Services;
using System.Security.Claims;

namespace R_StudioAPI.Controllers
{
    [Route("api/posts/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMediaService _mediaService;
        private readonly ApplicationConfig _appConfig;

        public PostController(UserManager<User> userManager, ApplicationDbContext applicationDbContext, IMediaService mediaService, IOptions<ApplicationConfig> appConfig)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _mediaService = mediaService;
            _appConfig = appConfig.Value;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(long id)
        {
            Post? post = await _applicationDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return BadRequest("Post not found");
            }

            List<PostMediaDto> mediaDtos = [];
            foreach (PostMedia mediaFile in post.Media)
            {
                mediaDtos.Add(new PostMediaDto()
                {
                    Id = mediaFile.Id,
                    Url = $"https://{Request.Host}/api/media/postmedia/{mediaFile.Url}",
                    PostId = mediaFile.PostId
                });
            }

            PostResponseDto postMediaDto = new PostResponseDto()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                CreatedOn = post.CreatedOn,
                Media = mediaDtos,
                Text = post.Text
            };


            return Ok(postMediaDto);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPost([FromForm] CreatePostRequestDto postDto)
        {
            User? user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.UserName == User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            List<PostMedia> media = new List<PostMedia>();

            if (postDto.MediaFiles != null)
            {
                foreach (IFormFile file in postDto.MediaFiles)
                {
                    if (!IsMediaFile(file))
                    {
                        return BadRequest("The file uploaded is not a media file!");
                    }
                }

                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{_appConfig}");

                foreach (IFormFile file in postDto.MediaFiles)
                {
                    string newFileName = $"{Guid.NewGuid()}-{DateTime.Now:dd.MM.yyyy-hh.mm.ss}{Path.GetExtension(file.FileName)}";

                    using FileStream fileStream = new FileStream($"{Directory.GetCurrentDirectory()}/{newFileName}", FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    media.Add(new PostMedia()
                    {
                        Url = newFileName
                    });
                }
            }

            Post post = new Post() { Author = user, Text = postDto.Text, CreatedOn = DateTime.Now, Media = media };

            _applicationDbContext.Posts.Add(post);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        private bool IsMediaFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return _mediaService.GetMediaExtensions().Contains(fileExtension);
        }
    }
}
