using R_StudioAPI.Models;

namespace R_StudioAPI.Dtos.Commentary
{
    public class CommentaryResponseDto
    {
        public long Id { get; set; }
        public long CommentatorId { get; set; }
        public long? PostId { get; set; }
        public long? VideoId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
