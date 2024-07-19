namespace R_StudioAPI.Models
{
    public class Favourite
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<Video> Videos { get; set; } = [];
        public virtual List<Post> Posts { get; set; } = [];
        public virtual List<User> Users { get; set; } = [];
    }
}
