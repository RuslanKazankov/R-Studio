using R_StudioAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace R_StudioAPI.Dtos.Post
{
    public class PostRequestDto
    {
        [Required]
        public string Text { get; set; } = String.Empty;
        [MaxLength(10)]
        public IFormFileCollection? MediaFiles { get; set; }
    }
}
