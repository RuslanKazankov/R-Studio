using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using R_StudioAPI.Config;
using R_StudioAPI.DAL.Interfaces;
using R_StudioAPI.Data;
using R_StudioAPI.Dtos.Pageable;
using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Extensions;
using R_StudioAPI.Mappers;
using R_StudioAPI.Models;
using R_StudioAPI.Repository.Interfaces;
using R_StudioAPI.Services;
using System.Security.Claims;

namespace R_StudioAPI.Controllers
{
    [Route("api/posts/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostUoW _postUoW;
        private readonly IMediaService _mediaService;
        private readonly ApplicationConfig _appConfig;
        private readonly IPostMapper _postMapper;

        public PostController(UserManager<User> userManager, IPostUoW postUoW, IMediaService mediaService, IOptions<ApplicationConfig> appConfig, IPostMapper postMapper)
        {
            _userManager = userManager;
            _postUoW = postUoW;
            _mediaService = mediaService;
            _appConfig = appConfig.Value;
            _postMapper = postMapper;
        }

        [HttpGet("get")]
        public IActionResult GetPosts([FromQuery] PageDto pageDto)
        {
            IEnumerable<Post> posts = _postUoW.PostRepository.GetPosts(pageDto.Page, pageDto.PageSize);

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
            Post? post = _postUoW.PostRepository.Get(id);

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
                    if (!_mediaService.IsMediaFile(file))
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

            await _postUoW.PostRepository.Create(post);
            await _postUoW.Save();

            return Ok();
        }


        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePost([FromForm] PostUpdateDto postRequestDto)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Post? targetPost = _postUoW.PostRepository.Get(postRequestDto.PostId);

            if (targetPost == null)
            {
                return BadRequest($"Post #{postRequestDto.PostId} not found");
            }

            if (postRequestDto.MediaFiles != null)
            {
                _postUoW.PostMediaRepository.RemoveList(targetPost.Media);

                List<PostMedia> media = new List<PostMedia>();

                foreach (IFormFile file in postRequestDto.MediaFiles)
                {
                    if (!_mediaService.IsMediaFile(file))
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

            if (postRequestDto.Text != null)
            {
                targetPost.Text = postRequestDto.Text;
            }

            await _postUoW.Save();

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

            Post? post = _postUoW.PostRepository.Get(id);

            if (post == null)
            {
                return BadRequest("Post not found");
            }

            _postUoW.PostRepository.Delete(id);
            await _postUoW.Save();

            return Ok("Post deleted");
        }
    }
}
