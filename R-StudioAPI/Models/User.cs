using Microsoft.AspNetCore.Identity;

namespace R_StudioAPI.Models
{
    public class User : IdentityUser<long>
    {
        //For Admin
        public virtual List<Post> MyPosts { get; set; } = []; 
        //For All
        public string? Avatar { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual List<Video> History { get; set; } = [];
        public virtual List<Video> LikedVideos { get; set; } = [];
        public virtual List<Post> LikedPosts { get; set; } = [];
        public long FavouriteId { get; set; }
        public virtual Favourite Favourite { get; set; } = new();
        public DateTime RegistratedOn { get; set; } = DateTime.Now;
    }
}
