namespace R_StudioAPI.Models
{
    public class Commentary
    {
        public long Id { get; set; }
        public long CommentatorId { get; set; }
        public virtual User Commentator { get; set; } = new();
        public long? PostId { get; set; }
        public virtual Post? Post { get; set; }
        public long? VideoId { get; set; }
        public virtual Video? Video { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
