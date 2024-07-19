namespace R_StudioAPI.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public long? AuthorId { get; set; }
        public virtual User? Author { get; set; } = new();
        public virtual List<PostMedia> Media { get; set; } = [];
        public virtual List<Commentary> Commentaries { get; set; } = [];
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
