namespace R_StudioAPI.Models
{
    public class PostMedia
    {
        public long Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public long PostId { get; set; }
        public virtual Post Post { get; set; } = new();
    }
}
