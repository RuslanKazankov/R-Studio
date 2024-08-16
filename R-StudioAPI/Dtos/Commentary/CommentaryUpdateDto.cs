using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Commentary
{
    public class CommentaryUpdateDto
    {
        [Required]
        public long CommentaryId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
