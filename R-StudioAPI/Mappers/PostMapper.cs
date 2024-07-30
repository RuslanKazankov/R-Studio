using Microsoft.Identity.Client;
using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Models;

namespace R_StudioAPI.Mappers
{
    public class PostMapper : IPostMapper
    {
        public PostResponseDto ToDto(Post post, string hostHeader)
        {
            List<PostMediaResponseDto> mediaDtos = [];
            foreach (PostMedia mediaFile in post.Media)
            {
                mediaDtos.Add(new PostMediaResponseDto()
                {
                    Id = mediaFile.Id,
                    Url = $"https://{hostHeader}/api/media/postmedia/{mediaFile.Url}",
                    PostId = mediaFile.PostId
                });
            }

            PostResponseDto postResponseDto = new PostResponseDto()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                CreatedOn = post.CreatedOn,
                Media = mediaDtos,
                Text = post.Text
            };

            return postResponseDto;
        }
    }
}
