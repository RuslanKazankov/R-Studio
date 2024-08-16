using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R_StudioAPI.DAL.Interfaces;
using R_StudioAPI.Dtos.Commentary;
using R_StudioAPI.Dtos.Pageable;
using R_StudioAPI.Extensions;
using R_StudioAPI.Mappers;
using R_StudioAPI.Models;
using R_StudioAPI.Repository.Interfaces;

namespace R_StudioAPI.Controllers
{
    [Route("api/commentaries/")]
    [ApiController]
    public class CommentaryController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICommentaryUoW _commentaryUoW;
        private readonly ICommentaryMapper _commentaryMapper;

        public CommentaryController(UserManager<User> userManager, ICommentaryUoW commentaryUoW, ICommentaryMapper commentaryMapper)
        {
            _userManager = userManager;
            _commentaryUoW = commentaryUoW;
            _commentaryMapper = commentaryMapper;
        }

        [HttpGet("get")]
        public IActionResult GetCommentaries([FromQuery] PageDto pageDto, [FromQuery] long? VideoId, [FromQuery] long? PostId)
        {
            if (PostId != null ^ VideoId == null)
            {
                return BadRequest("One PostId or VideoId parameter was expected");
            }

            List<CommentaryResponseDto> commentaryDtos = [];
            if (PostId != null)
            {
                Post? post = _commentaryUoW.PostRepository.Get(PostId.Value);

                if (post == null)
                {
                    return BadRequest("Post not found");
                }

                var commentaries = _commentaryUoW.CommentaryRepository.GetListByPostId(PostId.Value, pageDto.Page, pageDto.PageSize);

                foreach (var commentary in commentaries)
                {
                    commentaryDtos.Add(_commentaryMapper.ToDto(commentary));
                }
            }
            else if (VideoId != null)
            {
                Video? video = _commentaryUoW.VideoRepository.Get(VideoId.Value);

                if (video == null)
                {
                    return BadRequest("Video not found");
                }

                var commentaries = _commentaryUoW.CommentaryRepository.GetListByVideoId(VideoId.Value, pageDto.Page, pageDto.PageSize);

                foreach (var commentary in commentaries)
                {
                    commentaryDtos.Add(_commentaryMapper.ToDto(commentary));
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
                Post? post = _commentaryUoW.PostRepository.Get(commentaryRequestDto.PostId.Value);

                if (post == null)
                {
                    return BadRequest("Post not found");
                }

                commentary = new Commentary() { Commentator = user, CreatedOn = DateTime.Now, Post = post, Text = commentaryRequestDto.Text };
            }
            else if (commentaryRequestDto.VideoId != null)
            {
                Video? video = _commentaryUoW.VideoRepository.Get(commentaryRequestDto.VideoId.Value);

                if (video == null)
                {
                    return BadRequest("Video not found");
                }

                commentary = new Commentary() { Commentator = user, CreatedOn = DateTime.Now, Video = video, Text = commentaryRequestDto.Text };
            }
            else return BadRequest("Unknown Error");

            await _commentaryUoW.CommentaryRepository.Create(commentary);
            await _commentaryUoW.Save();
            return Ok();
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentary([FromBody] CommentaryUpdateDto commentaryUpdateDto)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Commentary? commentary = _commentaryUoW.CommentaryRepository.Get(commentaryUpdateDto.CommentaryId);

            if (commentary == null)
            {
                return BadRequest("Commentary not found");
            }

            if (commentary.CommentatorId != user.Id)
            {
                return BadRequest("You can only edit your own comments");
            }

            commentary.Text = commentaryUpdateDto.Text;

            await _commentaryUoW.Save();

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentary([FromQuery] long commentaryId)
        {
            User? user = await _userManager.FindByNameAsync(User.GetUsername());

            if (user == null)
            {
                return Unauthorized();
            }

            Commentary? commentary = _commentaryUoW.CommentaryRepository.Get(commentaryId);

            if (commentary == null)
            {
                return BadRequest("Commentary not found");
            }

            if (commentary.CommentatorId != user.Id && ! await user.IsAdmin(_userManager))
            {
                return BadRequest("You can only delete your own comments");
            }

            _commentaryUoW.CommentaryRepository.Delete(commentaryId);
            await _commentaryUoW.Save();

            return Ok();
        }

    }
}
