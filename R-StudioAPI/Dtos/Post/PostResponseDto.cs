using R_StudioAPI.Models;

namespace R_StudioAPI.Dtos.Post
{
    public class PostResponseDto
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public long? AuthorId { get; set; }
        public List<PostMediaDto> Media { get; set; } = [];
        public DateTime CreatedOn { get; set; }
    }
}
