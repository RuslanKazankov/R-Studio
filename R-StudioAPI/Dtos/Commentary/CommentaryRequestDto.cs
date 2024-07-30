using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Commentary
{
    public class CommentaryRequestDto
    {
        public long? PostId{ get; set; }
        public long? VideoId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
