using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using R_StudioAPI.Config;
using R_StudioAPI.Data;
using R_StudioAPI.Dtos.Pageable;
using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Extensions;
using R_StudioAPI.Mappers;
using R_StudioAPI.Models;
using R_StudioAPI.Repository;
using R_StudioAPI.Services;
using System.Security.Claims;

namespace R_StudioAPI.Controllers
{
    [Route("api/posts/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IPostMediaRepository _postMediaRepository;
        private readonly IMediaService _mediaService;
        private readonly ApplicationConfig _appConfig;
        private readonly IPostMapper _postMapper;

        public PostController(UserManager<User> userManager, IPostRepository postRepository, IPostMediaRepository postMediaRepository, IMediaService mediaService, IOptions<ApplicationConfig> appConfig, IPostMapper postMapper)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _postMediaRepository = postMediaRepository;
            _mediaService = mediaService;
            _appConfig = appConfig.Value;
            _postMapper = postMapper;
        }

        [HttpGet("get")]
        public IActionResult GetPosts([FromQuery] PageDto pageDto)
        {
            IEnumerable<Post> posts = _postRepository.GetPosts(pageDto.Page, pageDto.PageSize);

            List<PostResponseDto> postsDtos = [];

            foreach (Post post in posts)
            {
                postsDtos.Add(_postMapper.ToDto(post, Request.Host.Value));
            }

            return Ok(postsDtos);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetPost(long id)
        {
            Post? post = _postRepository.Get(id);

            if (post == null)
            {
                return BadRequest("Post not found");
            }

            PostResponseDto postResponseDto = _postMapper.ToDto(post, Request.Host.Value);

            return Ok(postResponseDto);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPost([FromForm] PostRequestDto postRequestDto)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            List<PostMedia> media = new List<PostMedia>();

            if (postRequestDto.MediaFiles != null)
            {
                foreach (IFormFile file in postRequestDto.MediaFiles)
                {
                    if (!IsMediaFile(file))
                    {
                        return BadRequest("The file uploaded is not a media file!");
                    }
                }

                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}");

                foreach (IFormFile file in postRequestDto.MediaFiles)
                {
                    string newFileName = $"{Guid.NewGuid()}-{DateTime.Now:dd.MM.yyyy-hh.mm.ss}{Path.GetExtension(file.FileName)}";

                    using FileStream fileStream = new FileStream($"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}{newFileName}", FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    media.Add(new PostMedia()
                    {
                        Url = newFileName
                    });
                }
            }

            Post post = new Post() { Author = user, Text = postRequestDto.Text, CreatedOn = DateTime.Now, Media = media };

            await _postRepository.Create(post);
            await _postRepository.Save();

            return Ok();
        }
        private bool IsMediaFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return _mediaService.GetMediaExtensions().Contains(fileExtension);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePost([FromBody] PostRequestDto postRequestDto, [FromBody] long postId)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Post? targetPost = _postRepository.Get(id);

            if (targetPost == null)
            {
                return BadRequest($"Post #{id} not found");
            }

            if (postRequestDto.MediaFiles != null)
            {
                _postMediaRepository.RemoveList(targetPost.Media);

                List<PostMedia> media = new List<PostMedia>();

                foreach (IFormFile file in postRequestDto.MediaFiles)
                {
                    if (!IsMediaFile(file))
                    {
                        return BadRequest("The file uploaded is not a media file!");
                    }
                }

                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}");

                foreach (IFormFile file in postRequestDto.MediaFiles)
                {
                    string newFileName = $"{Guid.NewGuid()}-{DateTime.Now:dd.MM.yyyy-hh.mm.ss}{Path.GetExtension(file.FileName)}";

                    using FileStream fileStream = new FileStream($"{Directory.GetCurrentDirectory()}/{_appConfig.PathPostMedia}{newFileName}", FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    media.Add(new PostMedia()
                    {
                        Url = newFileName
                    });
                }

                targetPost.Media = media;
            }

            targetPost.Text = postRequestDto.Text;

            await _postRepository.Save();

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(long id, bool confirm)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            if (!confirm)
            {
                return BadRequest("Not confirmed");
            }

            Post? post = _postRepository.Get(id);

            if (post == null)
            {
                return BadRequest("Post not found");
            }

            _postRepository.Delete(id);

            return Ok("Post deleted");
        }
    }
}
