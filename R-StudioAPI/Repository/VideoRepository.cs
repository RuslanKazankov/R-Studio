using R_StudioAPI.Data;
using R_StudioAPI.Models;

namespace R_StudioAPI.Repository
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public VideoRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
