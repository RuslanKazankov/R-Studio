using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Models;

namespace R_StudioAPI.Mappers
{
    public class PostMapper : IPostMapper
    {
        public PostResponseDto ToDto(Post post, string hostHeader)
        {
            List<PostMediaDto> mediaDtos = [];
            foreach (PostMedia mediaFile in post.Media)
            {
                mediaDtos.Add(new PostMediaDto()
                {
                    Id = mediaFile.Id,
                    Url = $"https://{hostHeader}/api/media/postmedia/file?filename={mediaFile.Url}",
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
