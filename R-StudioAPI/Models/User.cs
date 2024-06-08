namespace R_StudioAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Role Role { get; set; } = new();
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Video> History { get; set; } = [];
        public List<Video> Liked { get; set; } = [];
    }
}
