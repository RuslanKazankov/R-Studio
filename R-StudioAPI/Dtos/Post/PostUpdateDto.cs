using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Post
{
    public class PostUpdateDto
    {
        [Required]
        public long PostId { get; set; }
        public string? Text { get; set; } = String.Empty;
        [MaxLength(10)]
        public IFormFileCollection? MediaFiles { get; set; }
    }
}
