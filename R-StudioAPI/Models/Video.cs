using System.ComponentModel.DataAnnotations.Schema;

namespace R_StudioAPI.Models
{
    public class Video
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public long Views { get; set; }
        public long Likes { get; set; }
        public long Rating { get; set; }
        public long RatingCount { get; set; }
        public virtual List<User> Actors { get; set; } = [];
        public virtual List<Commentary> Commentaries { get; set; } = [];
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
