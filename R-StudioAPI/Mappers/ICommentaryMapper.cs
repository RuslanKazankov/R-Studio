using R_StudioAPI.Dtos.Commentary;
using R_StudioAPI.Models;

namespace R_StudioAPI.Mappers
{
    public interface ICommentaryMapper
    {
        CommentaryResponseDto ToDto(Commentary commentary);
    }
}
