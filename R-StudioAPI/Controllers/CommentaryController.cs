using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R_StudioAPI.Dtos.Commentary;
using R_StudioAPI.Dtos.Pageable;
using R_StudioAPI.Extensions;
using R_StudioAPI.Mappers;
using R_StudioAPI.Models;
using R_StudioAPI.Repository;

namespace R_StudioAPI.Controllers
{
    [Route("api/commentaries/")]
    [ApiController]
    public class CommentaryController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICommentaryRepository _commentaryRepository;
        private readonly IPostRepository _postRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly ICommentaryMapper _commentaryMapper;

        public CommentaryController(UserManager<User> userManager, ICommentaryRepository commentaryRepository, IPostRepository postRepository,
            IVideoRepository videoRepository, ICommentaryMapper commentaryMapper)
        {
            _userManager = userManager;
            _commentaryRepository = commentaryRepository;
            _postRepository = postRepository;
            _videoRepository = videoRepository;
            _commentaryMapper = commentaryMapper;
        }

        [HttpGet("get")]
        public IActionResult GetCommentaries([FromQuery] PageDto pageDto, [FromQuery] long? VideoId, [FromQuery] long? PostId)
        {
            if (PostId == null ^ VideoId == null)
            {
                return BadRequest("One PostId or VideoId parameter was expected");
            }

            IEnumerable<CommentaryResponseDto> commentaryDtos = [];
            if (PostId != null)
            {
                Post? post = _postRepository.Get(PostId.Value);

                if (post == null)
                {
                    return BadRequest("Post not found");
                }

                var commentaries = _commentaryRepository.GetListByPostId(PostId.Value, pageDto.Page, pageDto.PageSize);

                foreach (var commentary in commentaries)
                {
                    commentaryDtos.Append(_commentaryMapper.ToDto(commentary));
                }
            }
            else if (VideoId != null)
            {
                Video? video = _videoRepository.Get(VideoId.Value);

                if (video == null)
                {
                    return BadRequest("Video not found");
                }

                var commentaries = _commentaryRepository.GetListByVideoId(VideoId.Value, pageDto.Page, pageDto.PageSize);

                foreach (var commentary in commentaries)
                {
                    commentaryDtos.Append(_commentaryMapper.ToDto(commentary));
                }
            }

            return Ok(commentaryDtos);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddCommentary([FromBody] CommentaryRequestDto commentaryRequestDto)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            if (commentaryRequestDto.PostId == null ^ commentaryRequestDto.VideoId == null)
            {
                return BadRequest("One PostId or VideoId parameter was expected");
            }

            Commentary commentary = new();

            if (commentaryRequestDto.PostId != null)
            {
                Post? post = _postRepository.Get(commentaryRequestDto.PostId.Value);

                if (post == null)
                {
                    return BadRequest("Post not found");
                }

                commentary = new Commentary() { Commentator = user, CreatedOn = DateTime.Now, Post = post, Text = commentaryRequestDto.Text };
            }
            else if (commentaryRequestDto.VideoId != null)
            {
                Video? video = _videoRepository.Get(commentaryRequestDto.VideoId.Value);

                if (video == null)
                {
                    return BadRequest("Video not found");
                }

                commentary = new Commentary() { Commentator = user, CreatedOn = DateTime.Now, Video = video, Text = commentaryRequestDto.Text };
            }
            else return BadRequest("Unknown Error");

            await _commentaryRepository.Create(commentary);
            await _commentaryRepository.Save();
            return Ok();
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentary([FromBody] long commentaryId, [FromBody] string newText)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Commentary? commentary = _commentaryRepository.Get(commentaryId);

            if (commentary == null)
            {
                return BadRequest("Commentary not found");
            }

            if (commentary.CommentatorId != user.Id)
            {
                return BadRequest("You can only edit your own comments");
            }

            commentary.Text = newText;

            await _commentaryRepository.Save();

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentary([FromBody] long commentaryId)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Commentary? commentary = _commentaryRepository.Get(commentaryId);

            if (commentary == null)
            {
                return BadRequest("Commentary not found");
            }

            if (commentary.CommentatorId != user.Id && ! await user.IsAdmin(_userManager))
            {
                return BadRequest("You can only delete your own comments");
            }

            _commentaryRepository.Delete(commentaryId);
            await _commentaryRepository.Save();

            return Ok();
        }

    }
}
