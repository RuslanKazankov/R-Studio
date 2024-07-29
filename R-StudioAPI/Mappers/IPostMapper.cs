using R_StudioAPI.Dtos.Post;
using R_StudioAPI.Models;

namespace R_StudioAPI.Mappers
{
    public interface IPostMapper
    {
        PostResponseDto ToDto(Post post, string postHeader);
    }
}
