using R_StudioAPI.Dtos.Commentary;
using R_StudioAPI.Models;

namespace R_StudioAPI.Mappers
{
    public class CommentaryMapper : ICommentaryMapper
    {
        public CommentaryResponseDto ToDto(Commentary commentary)
        {
            return new() {
                Id = commentary.Id,
                CommentatorId = commentary.CommentatorId,
                VideoId = commentary.VideoId,
                PostId = commentary.PostId,
                Text = commentary.Text,
                CreatedOn = commentary.CreatedOn
            };
        }
    }
}
